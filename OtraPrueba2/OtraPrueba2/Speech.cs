using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;
using System.Collections.Generic;

namespace OtraPrueba2
{
    public class Speech
    {
        static SpeechSynthesizer ss = new SpeechSynthesizer(); // Creo que esto no se usa nunca
        static SpeechRecognitionEngine sreComandos;
        static SpeechRecognitionEngine sreTeclado;
        static DictationGrammar dictation;
        static bool done = false; // Creo que esto no se usa nunca
        static bool speechOn = true;
        public static void Initialize()
        {
            try
            {
                ss.SetOutputToDefaultAudioDevice(); // Creo que esto no se usa nunca
                CultureInfo ci = new CultureInfo("es-ES"); // Para que el idioma de la entrada de voz sea espaniol

                //Comandos
                sreComandos = new SpeechRecognitionEngine(ci);
                sreComandos.SetInputToDefaultAudioDevice();

                //Teclado <-- NO LO TERMINE YET
                //sreTeclado = new SpeechRecognitionEngine(ci);
                //sreTeclado.SetInputToDefaultAudioDevice();
                //dictation = new DictationGrammar();
                //dictation.Name = "default dictation";
                //dictation.Enabled = true;
                //sreTeclado.LoadGrammar(dictation);
                //sreTeclado.SpeechRecognized += sreTeclado_SpeechRecognized;
                //sreTeclado.RecognizeAsync(RecognizeMode.Multiple);

                Choices choices = new Choices(); // Opciones para decir
                choices.Add("click");
                choices.Add("doble click");
                choices.Add("click derecho");
                choices.Add("seleccionar");
                choices.Add("desseleccionar");
                choices.Add("desconectar");
                choices.Add("silencio");
                choices.Add("activar teclado"); 

                sreComandos.SpeechRecognized += sreComandos_SpeechRecognized; //Cuando reconoce alguna de las opciones, va a la funcion
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(choices);
                Grammar grammar = new Grammar(grammarBuilder);
                sreComandos.LoadGrammar(grammar);
                sreComandos.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        } // Main

        public static void sreTeclado_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text);
        }

        static bool bSilencio = false;
        static void sreComandos_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            
            string txt = e.Result.Text;
            Console.WriteLine("\nRecognized: " + txt);

            float confidence = e.Result.Confidence; // Esto dejenlo, porque hay veces que si hay un ruido entiende cualquier cosa
            if (confidence < 0.60)
            {
                if (!bSilencio)
                    mostrar_Decir_Mensaje("No se te entendio una goma, habla de nuevo");
            }

            if (txt.Equals("click"))
            {
                MouseHook.ClickOnPoint(1, true);
            }
            else if (txt.Equals("doble click"))
            {
                MouseHook.ClickOnPoint(2, true);
            }
            else if (txt.Equals("click derecho"))
            {
                MouseHook.ClickOnPoint(1, false);
            } else if (txt.Equals("seleccionar")) {
                MouseHook.HoldOnPoint(true);
            } else if (txt.Equals("desseleccionar")) {
                MouseHook.HoldOnPoint(false);
            } else if(txt.Equals("activar teclado"))
            {
                //Aca se va a activar y desactivar el teclado
            }
            else if (txt.Equals("desconectar"))
            {
                if (!bSilencio) { 
                    mostrar_Decir_Mensaje("Gracias por utilizar aijansi");
                    Test.Desconectar();
                }
            }
            else if (txt.Equals("silencio"))
            {
                if (bSilencio)
                {
                    mostrar_Decir_Mensaje("Silencio desactivado");
                    bSilencio = false;
                } else
                {
                    mostrar_Decir_Mensaje("Silencio activado");
                    bSilencio = true;
                }
            }
            else // No pasa nunca, podemos borrarla o dejarla porque esta bueno que putee en ingles (me esforce mucho xD)
            {
                mostrar_Decir_Mensaje(txt);
            }
            if (speechOn == false) return;
        } // sreComandos_SpeechRecognized

        static void mostrar_Decir_Mensaje(String texto)
        {
            String textoMostrarDecir = "";
            bool espaniolInstalado = false;
            int iContVoces = 0;
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            IList < InstalledVoice > vocesInstaladas = synthesizer.GetInstalledVoices(); // Lista de las voces instaladas en la PC
            while ((espaniolInstalado == false) && (iContVoces < vocesInstaladas.Count))
            {
                if (vocesInstaladas[iContVoces].VoiceInfo.Description.Contains("Microsoft Helena Desktop")) // Nombre de la voz en español
                {
                    espaniolInstalado = true;
                }
                else
                {
                    iContVoces++;
                }
            }
            
            if (espaniolInstalado)
            {
                synthesizer.SelectVoice("Microsoft Helena Desktop"); // Si la PC tiene la voz en español, se la asigno al "hablador"
                Char primeraLetra = Char.ToUpper(texto[0]); // Convierte la primera letra del texto a mayuscula
                textoMostrarDecir = primeraLetra + texto.Substring(1, texto.Length - 1) + " realizado. "; // Concatena la primera letra en mayuscula con el resto del texto
            }
            else
            {
                textoMostrarDecir = "Why don't you install the Spanish package, you fucking son of a bitch? By the way, Iron Man dies."; // Amén
            }

            //Console.WriteLine(textoMostrarDecir);
            synthesizer.Speak(textoMostrarDecir);
            synthesizer.Dispose();
            speechOn = true;
        }
    } // Program
} // ns