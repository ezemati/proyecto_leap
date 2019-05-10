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
                Thread.Sleep(500); // Espera porque sino el controller no llega a conectarse con el dispositivo
                if (!leapReader.leapEstaConectado())
                {
                    Console.WriteLine("No conectado");
                    MessageBox.Show("El Leap no esta conectado!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //Activa keylogger
                KeyboardHook.Initialize();

                //Activa control de mouse
                MouseHook.Initialize(handle);

                //Activa registro de voz
                Speech.Initialize();

                Application.Run();

                KeyboardHook.Dispose();
                MouseHook.Dispose();
            } // Fin de verificar si esta instalado
            else
            {                
                Console.WriteLine("No instalado");
                MessageBox.Show("No instalaste la aplicacion de Leap!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // Fin del main
    
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