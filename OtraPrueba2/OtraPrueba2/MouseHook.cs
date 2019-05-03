using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OtraPrueba2
{
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

        #region codigosMouse
        private const int mouse_LeftDown = 0x02;
        private const int mouse_LeftUp = 0x04;
        private const int mouse_RightDown = 0x08;
        private const int mouse_RightUp = 0x10;
        #endregion

        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelMouseProc _proc = HookCallback;

        private const int WH_MOUSE_LL = 14;

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr handle;

        public static void Initialize(IntPtr h)
        {
            handle = h;
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
            if (nCode >= 0 && MouseMessage.WM_LBUTTONDOWN == (MouseMessage)wParam)
            {
                MSLLHOOK hookStruct = (MSLLHOOK)Marshal.PtrToStructure(lParam, typeof(MSLLHOOK));
                Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
                //SetCursorPos(hookStruct.pt.x + 5, hookStruct.pt.y);//Mueve el mouse 5pxs para la derecha

                //LeftMouseClicks++;
                //Console.WriteLine(LeftMouseClicks);
            }
            else if (nCode >= 0 && MouseMessage.WM_RBUTTONDOWN == (MouseMessage)wParam)
            {
                //RightMouseClicks++;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        #region structsImportados
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
        #endregion

        #region dllsImportados
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        #endregion

        public static void ClickOnPoint(int iCantClicks, bool esIzq)
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            if (esIzq)
            {
                for (int i = 1; i <= iCantClicks; i++)
                {
                    mouse_event(mouse_LeftDown, X, Y, 0, 0);
                    mouse_event(mouse_LeftUp, X, Y, 0, 0);
                    Thread.Sleep(50); // Necesario para que haya separacion entre cada click
                }
            }
            else
            {
                mouse_event(mouse_RightDown, X, Y, 0, 0);
                mouse_event(mouse_RightUp, X, Y, 0, 0);
            }
        }

        public static void HoldOnPoint(bool bHold)
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            if (bHold) { 
                mouse_event(mouse_LeftDown, X, Y, 0, 0);
            } else
            {
                mouse_event(mouse_LeftUp, X, Y, 0, 0);
            }
        }
    }
}
