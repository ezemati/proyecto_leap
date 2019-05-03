using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace OtraPrueba2 {
    class Test
    {
        public static void Main()
        {
            var handle = GetConsoleWindow(); // Obtener la ventana

            //Activa el controlador del leap motion
            LeapController leapReader = new LeapController();

            if (leapEstaInstalado())
            {
                Thread.Sleep(500); // Que espere porque sino el controller no llega a conectarse con el dispositivo
                if (!leapReader.leapEstaConectado())
                {
                    //MessageBox.Show("Test");
                    //Application.Exit();
                    Console.WriteLine("No conectado");
                }
                else
                {
                    //Activa keylogger
                    KeyboardHook.Initialize();

                    //Activa control de mouse
                    MouseHook.Initialize(handle);

                    //Activa registro de voz
                    Speech.Initialize();

                    Application.Run();

                    KeyboardHook.Dispose();
                    MouseHook.Dispose();
                }
            }
            else
            {
                //MessageBox.Show("Test");
                //Application.Exit();
                Console.WriteLine("No instalado");
            }
        }
    
        public static void Desconectar()
        {
            Application.Exit();
        }

        public static bool leapEstaInstalado()
        {
            bool estaInstalado = false;
            string direccionRegistro = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Leap Services";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(direccionRegistro);
            if (key != null && key.GetValue("DisplayName").Equals("Leap Motion Software"))
            {
                estaInstalado = true;
            }
            return estaInstalado;
        }

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
    }
}