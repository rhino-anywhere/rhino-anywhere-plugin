using Rhino;
using Rhino.Commands;
using Rhino.Display;
using RhinoAnywhere.DataStructures;
using SIPSorcery.Media;
using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using SIPSorceryMedia.Encoders;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace RhinoAnywhere
{

  public class Server : IDisposable
  {
    private int WEBSOCKET_PORT { get; set; } = 2337;
    public uint DurationUnits { get; set; } = 1;
    public RTCPeerConnection Connection { get; set; }
    private WebSocketServer SocketServer { get; set; }
    private VpxVideoEncoder Encoder;

    public Server()
    {
      var port = WEBSOCKET_PORT;
      SocketServer = new WebSocketServer(IPAddress.Any, port);
      SocketServer.AddWebSocketService<WebRTCWebSocketPeer>("/", (peer) => peer.CreatePeerConnection = () => CreatePeerConnection());
      SocketServer.Start();
      RhinoApp.WriteLine($"Listening for connections on 0.0.0.0:{port}");
      Encoder = new VpxVideoEncoder();
    }

    public void Dispose()
    {
      Connection?.Close("Command Rerun");
      SocketServer?.Stop();
    }

    private Task<RTCPeerConnection> CreatePeerConnection()
    {
      Connection = new RTCPeerConnection(null);

      var testPatternSource = new VideoTestPatternSource(Encoder);

      MediaStreamTrack videoTrack = new MediaStreamTrack(testPatternSource.GetVideoSourceFormats(), MediaStreamStatusEnum.SendOnly);
      Connection.addTrack(videoTrack);

      // testPatternSource.OnVideoSourceEncodedSample += connection.SendVideo;
      Connection.OnVideoFormatsNegotiated += (formats) => testPatternSource.SetVideoSourceFormat(formats.First());

      Connection.onconnectionstatechange += async (state) =>
      {
        Console.WriteLine($"Peer connection state change to {state}.");

        switch (state)
        {
          case RTCPeerConnectionState.connected:
            await testPatternSource.StartVideo();
            break;
          case RTCPeerConnectionState.failed:
            // Connection failed - attempt to reconnect or clean up
            RhinoApp.WriteLine("Connection failed, attempting to reconnect...");
            await AttemptReconnect();
            break;
          case RTCPeerConnectionState.closed:
            await testPatternSource.CloseVideo();
            testPatternSource.Dispose();
            break;
        }
      };

      Connection.createDataChannel("test");

      Connection.ondatachannel += (channel) =>
      {
        channel.onmessage += (test1, something, data) =>
              {
                string json = System.Text.Encoding.UTF8.GetString(data);
                var tst = JsonSerializer.Deserialize<JsonObject>(json);

                string type = tst["type"].ToString();
                Action<string> method = type switch
                {
                  "command" => MessageHandler.HandleCommand,
                  "input" => MessageHandler.HandleClick,
                  "resize" => MessageHandler.HandleResize,
                  "scroll" => MessageHandler.HandleScroll,
                  _ => throw new NotImplementedException("No"),
                };

                method(json);
              };
      };

      async Task AttemptReconnect()
      {
        try
        {
          // Close the existing connection if it's not already closed
          Connection?.Close("Close before reconnect");

          // Reinitialize the connection
          Connection = await CreatePeerConnection();

          RhinoApp.WriteLine("Reconnection successful");
        }
        catch (Exception ex)
        {
          RhinoApp.WriteLine($"Reconnection failed: {ex.Message}");
          // Handle reconnection failure as appropriate
        }
      }

      RhinoApp.Idle += InitialReDraw;

      return Task.FromResult(Connection);
    }

    private void InitialReDraw(object sender, EventArgs e)
    {
      System.Threading.Thread.Sleep(1000);
      RhinoApp.Idle -= InitialReDraw;
      RhinoDoc.ActiveDoc.Views.Redraw();
    }

    public bool SendBitmap(Bitmap bitmap)
    {
      var rect = new Rectangle(new Point(0, 0), new Size(bitmap.Width, bitmap.Height));
      var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

      try
      {
        IntPtr ptr = bitmapData.Scan0;
        int bytes = bitmapData.Stride * bitmap.Height;
        byte[] rgbValues = new byte[bytes];

        Marshal.Copy(ptr, rgbValues, 0, bytes);

        var encodedVideo = Encoder.EncodeVideo(bitmap.Width, bitmap.Height, rgbValues, VideoPixelFormatsEnum.Bgra, VideoCodecsEnum.H265);
        Connection.SendVideo(DurationUnits, encodedVideo);
        return true;
      }
      catch (Exception ex)
      {
        RhinoApp.WriteLine($"Video encoding or sending failed: {ex.Message}");
        // Handle the exception, potentially reinitialize the encoder or alert the user
      }
      finally
      {
        bitmap.UnlockBits(bitmapData);
      }

      return false;
    }

  }
}