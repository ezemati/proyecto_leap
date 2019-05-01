using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;

namespace OtraPrueba2
{
    public class Speech
    {
        static SpeechSynthesizer ss = new SpeechSynthesizer();
        static SpeechRecognitionEngine sre;
        static bool done = false;
        static bool speechOn = true;
        public static void Initialize()
        {
            try
            {
                ss.SetOutputToDefaultAudioDevice();
                CultureInfo ci = new CultureInfo("es-ES"); // Para que el idioma de la entrada de voz sea espaniol
                sre = new SpeechRecognitionEngine(ci);
                sre.SetInputToDefaultAudioDevice();
                Choices choices = new Choices(); // Opciones para decir
                choices.Add("speech on");
                choices.Add("speech off");
                choices.Add("click");
                choices.Add("doble click");
                choices.Add("click derecho");
                sre.SpeechRecognized += sre_SpeechRecognized; //Cuando reconoce alguna de las opciones, va a la funcion
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(choices);
                Grammar grammar = new Grammar(grammarBuilder);
                //sre.LoadGrammarAsync(grammar); --> En internet me aparecia siempre de la forma de abajo (Saidman)
                sre.LoadGrammar(grammar);
                sre.RecognizeAsync(RecognizeMode.Multiple);
                Console.WriteLine("\nHit <enter> to close shell\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        } // Main
        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            Console.WriteLine("\nRecognized: " + txt);

            //Si el sistema no esta seguro de lo que entendio, no se hace nada (lo de confidence)
            //float confidence = e.Result.Confidence;
            //if (confidence < 0.60) return;

            // Propongo borrar esto de Speech =) (Saidman)
            if (txt.Contains("speech on"))
            {
                mostrar_Decir_Mensaje("Speech is now ON");
            }
            else if (txt.Contains("speech off"))
            {
                mostrar_Decir_Mensaje("Speech is now OFF");
            }
            else if (txt.Contains("click"))
            {
                MouseHook.ClickOnPoint(); //Prueba de click
            } else
            {
                mostrar_Decir_Mensaje(txt);
            }
            if (speechOn == false) return;
        } // sre_SpeechRecognized

        static void mostrar_Decir_Mensaje(String texto)
        {
            Char primeraLetra = Char.ToUpper(texto[0]); // Convierte la primera letra del texto a mayuscula
            String textoMostrarDecir = primeraLetra + texto.Substring(1, texto.Length - 1) + " realizado. "; // Concatena la primera letra en mayuscula con el resto del texto
            Console.WriteLine(textoMostrarDecir);
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Speak(textoMostrarDecir);
            synthesizer.Dispose();
            speechOn = true;
        }
    } // Program
} // ns