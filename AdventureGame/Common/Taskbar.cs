using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Common
{
    public class Taskbar
    {
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);
        [DllImport("user32.dll")]
        private static extern int FindWindowEx(int parentHandle, int childAfter, string className, int windowTitle);
        [DllImport("user32.dll")]
        private static extern int GetDesktopWindow();

        private const int HIDE = 0;
        private const int SHOW = 1;

        protected static int Handle { get { return FindWindow("Shell_TrayWnd", ""); } }
        protected static int StartButtonHandle
        {
            get
            {
                var desktopHandle = GetDesktopWindow();
                var startButtonHandle = FindWindowEx(desktopHandle, 0, "button", 0);
                return startButtonHandle;
            }
        }

        public static void Show()
        {
            ShowWindow(Handle, SHOW);
            ShowWindow(StartButtonHandle, SHOW);
        }

        public static void Hide()
        {
            ShowWindow(Handle, HIDE);
            ShowWindow(StartButtonHandle, HIDE);
        }

        private Taskbar() { }
    }
}
