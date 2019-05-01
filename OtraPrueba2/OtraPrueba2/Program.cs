using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OtraPrueba2 {
    class Test
    {
        public static void Main()
        {
            var handle = GetConsoleWindow();
            // Ocultar la ventana
            KeyboardHook.Initialize();
            MouseHook.Initialize();
            Application.Run();
            KeyboardHook.Dispose();
            MouseHook.Dispose();

        }

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
    }
}