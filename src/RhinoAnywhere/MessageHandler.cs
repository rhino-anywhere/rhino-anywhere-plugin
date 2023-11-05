using Rhino;
using RhinoAnywhere.DataStructures;
using System;
using System.Drawing;
using System.Text.Json;

namespace RhinoAnywhere
{

  public class MessageHandler
  {
    private static string lastcommand { get; set; }

    public static void HandleCommand(string json)
    {
      var clickPacket = JsonSerializer.Deserialize<Packet<CommandData>>(json);
      lastcommand = clickPacket.data.command;
      RhinoApp.WriteLine(lastcommand);
      RhinoApp.Idle += RhinoApp_Idle;
    }

    public static void RhinoApp_Idle(object sender, EventArgs e)
    {
      RhinoApp.Idle -= RhinoApp_Idle;
      RhinoApp.RunScript(lastcommand, true);
    }

    public static void HandleClick(string json)
    {
      var clickPacket = JsonSerializer.Deserialize<Packet<MouseData>>(json);
      InputRecieved(clickPacket);
    }

    public static void HandleResize(string json)
    {
      var viewportSize = JsonSerializer.Deserialize<Packet<ViewportSize>>(json);
      RhinoDoc.ActiveDoc.Views.ActiveView.Size = new Size((int)viewportSize.data.Width, (int)viewportSize.data.Height);
    }
    public static void HandleScroll(string json)
    {
      var scrollAmount = JsonSerializer.Deserialize<Packet<ScrollData>>(json);
      MouseController.MousewheelScroll(-(int)scrollAmount.data.amount);
    }

    public static void InputRecieved(Packet<MouseData> inputArgs)
    {
      if (inputArgs.type == "input")
      {

        string val = inputArgs.data.value;

        string left = "0";
        string right = "2";
        if (inputArgs.data.method == "mouse")
        {
          if (inputArgs.data.action == "up" && val == left)
          {
            MouseController.MouseEvent(MouseController.MouseEventFlags.LeftUp);
          }
          else if (inputArgs.data.action == "down" && val == left)
          {
            MouseController.MouseEvent(MouseController.MouseEventFlags.LeftDown);
          }
          else if (inputArgs.data.action == "down" && val == right)
          {
            MouseController.MouseEvent(MouseController.MouseEventFlags.RightDown);
          }
          else if (inputArgs.data.action == "up" && val == right)
          {
            MouseController.MouseEvent(MouseController.MouseEventFlags.RightUp);
          }
          else if (inputArgs.data.action == "move")
          {
            double newX = inputArgs.data.x + inputArgs.data.deltax;
            double newY = inputArgs.data.y + inputArgs.data.deltay;

            var pt = DisplayController.WebViewToServerWindowCoordinate(newY, newX);
            MouseController.SetCursorPosition((int)pt.X, (int)pt.Y);
          }
        }
        else if (inputArgs.data.method == "keyboard")
        {
          // maybe keys
          int keyCode = int.Parse(inputArgs.data.value);
          bool up = inputArgs.data.action.ToLower() == "keyup";
          MouseController.KeyboardEvent(keyCode, up);
        }
      }
    }
  }
}
