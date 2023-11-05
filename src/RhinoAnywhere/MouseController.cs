using System;
using System.Runtime.InteropServices;

namespace RhinoAnywhere
{

  /// <summary>Handles Mouse Controlling</summary>
  public static class MouseController
  {
    [Flags]
    public enum MouseEventFlags
    {
      LeftDown = 0x00000002,
      LeftUp = 0x00000004,
      MiddleDown = 0x00000020,
      MiddleUp = 0x00000040,
      Move = 0x00000001,
      Absolute = 0x00008000,
      RightDown = 0x00000008,
      RightUp = 0x00000010,
      WheelScroll = 0x0800
    }

    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out MousePoint lpMousePoint);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, long dwFlags, long dwExtraInfo);

    /// <summary>Sets a Cursor Position</summary>
    /// <param name="x">The X coordinate</param>
    /// <param name="y">The Y Cooordinate</param>
    public static void SetCursorPosition(int x, int y)
    {
      SetCursorPos(x, y);
    }

    public static void MousewheelScroll(int amount)
    {
      MouseEvent(MouseEventFlags.WheelScroll, amount);
    }

    /// <summary>Sets a Cursor Position</summary>
    public static void SetCursorPosition(MousePoint point)
        => SetCursorPos(point.X, point.Y);

    /// <summary>Returns the current cursor position</summary>
    public static MousePoint GetCursorPosition()
    {
      MousePoint currentMousePoint;
      var gotPoint = GetCursorPos(out currentMousePoint);
      if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
      return currentMousePoint;
    }

    /// <summary>Captures a Mouse Event</summary>
    public static void MouseEvent(MouseEventFlags value, int dwData = 0)
    {
      MousePoint position = GetCursorPosition();

      mouse_event
          ((int)value,
           position.X,
           position.Y,
           dwData,
           0)
          ;
    }

    public static void KeyboardEvent(int key, bool up = false)
    {
        long flags = up ? 0x0002 : 0;
        keybd_event((byte)key, 0, flags, 0);
    }

    /// <summary>Maps a Mouse Coordinate to a struct</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
      public int X;
      public int Y;

      public MousePoint(int x, int y)
      {
        X = x;
        Y = y;
      }
    }
  }
}
