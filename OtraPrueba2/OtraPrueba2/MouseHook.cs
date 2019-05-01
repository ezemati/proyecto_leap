using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtraPrueba2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public enum MouseMessage
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }

    public static class MouseHook
    {
        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelMouseProc _proc = HookCallback;

        private const int WH_MOUSE_LL = 14;

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static long LeftMouseClicks { get; private set; }
        public static long RightMouseClicks { get; private set; }

        public static void Initialize()
        {
            _hookID = SetHook(_proc);
        }

        public static void Dispose()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule currentModule = currentProcess.MainModule)
                    return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(currentModule.ModuleName), 0);
            }
        }

        //https://blogs.msdn.microsoft.com/toub/2006/05/03/low-level-mouse-hook-in-c/
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessage.WM_LBUTTONDOWN == (MouseMessage)wParam) { 
                MSLLHOOK hookStruct = (MSLLHOOK)Marshal.PtrToStructure(lParam, typeof(MSLLHOOK));
                Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
                SetCursorPos(hookStruct.pt.x+5, hookStruct.pt.y);//Mueve el mouse 5pxs para la derecha
                    
                //LeftMouseClicks++;
                //Console.WriteLine(LeftMouseClicks);
            }
            else if (nCode >= 0 && MouseMessage.WM_RBUTTONDOWN == (MouseMessage)wParam) { 
                //RightMouseClicks++;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT

        {

            public int x;

            public int y;

        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOK

        {

            public POINT pt;

            public uint mouseData;

            public uint flags;

            public uint time;

            public IntPtr dwExtraInfo;

        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
