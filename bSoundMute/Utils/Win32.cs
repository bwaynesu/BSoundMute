using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace bSoundMute.Utils
{
  internal class Win32
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left;        // x position of upper-left corner
      public int Top;         // y position of upper-left corner
      public int Right;       // x position of lower-right corner
      public int Bottom;      // y position of lower-right corner
    }

    public struct MARGINS
    {
      public int m_Left;
      public int m_Right;
      public int m_Top;
      public int m_Buttom;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public int X;
      public int Y;

      public POINT(int x, int y)
      {
        this.X = x;
        this.Y = y;
      }

      public static implicit operator Point(POINT p)
      {
        return new Point(p.X, p.Y);
      }

      public static implicit operator POINT(Point p)
      {
        return new POINT(p.X, p.Y);
      }
    }

    public const uint WS_CHILD = 0x40000000;
    public const uint WS_EX_LAYERED = 0x00080000;
    public const uint WS_CLIPSIBLINGS = 0x4000000;
    public const uint WM_ACTIVATEAPP = 28;
    public const int WM_SIZE = 5;
    public const UInt32 SWP_NOSIZE = 0x0001;
    public const UInt32 SWP_NOMOVE = 0x0002;
    public const UInt32 SWP_SHOWWINDOW = 0x0040;
    public const uint SW_RESTORE = 0x09;
    public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    public static int version = Environment.OSVersion.Version.Major;

    public static bool DwmIsCompositionEnabled
    {
      get
      {
        if (version >= 6)
        {
          return DwmIsCompositionEnabled32(); // return true if Windows supports aero and it's enabled.
        }
        else
        {
          return false;
        }
      }
    }

    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(POINT Point);

    [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("dwmapi.dll", EntryPoint = "DwmIsCompositionEnabled", PreserveSig = false)]
    private static extern bool DwmIsCompositionEnabled32();

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int GetWindowText(int hWnd, StringBuilder text, int count);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    public static extern int ShowWindow(IntPtr hWnd, uint Msg);

    [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
    public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margin);
  }
}
