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
                CultureInfo ci = new CultureInfo("es-es");
                sre = new SpeechRecognitionEngine(ci);
                sre.SetInputToDefaultAudioDevice();
                sre.SpeechRecognized += sre_SpeechRecognized;
                Choices ch_StartStopCommands = new Choices();
                ch_StartStopCommands.Add("speech on");
                ch_StartStopCommands.Add("speech off");
                ch_StartStopCommands.Add("click");
                ch_StartStopCommands.Add("doble click");
                ch_StartStopCommands.Add("click derecho");
                GrammarBuilder gb_StartStop = new GrammarBuilder();
                gb_StartStop.Append(ch_StartStopCommands);
                Grammar g_StartStop = new Grammar(gb_StartStop);
                sre.LoadGrammarAsync(g_StartStop);
                sre.RecognizeAsync(RecognizeMode.Multiple);
                while (done == false) {; }
                Console.WriteLine("\nHit <enter> to close shell\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        } // Main
        static void sre_SpeechRecognized(object sender,
            SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float confidence = e.Result.Confidence;
            Console.WriteLine("\nRecognized: " + txt);
            if (confidence < 0.60) return;
            if (txt.IndexOf("speech on") >= 0)
            {
                Console.WriteLine("Speech is now ON");
                speechOn = true;
            }
            if (txt.IndexOf("speech off") >= 0)
            {
                Console.WriteLine("Speech is now OFF");
                speechOn = false;
            }
            if (txt.IndexOf("click derecho") >= 0)
            {
                Console.WriteLine("click derecho realizado");
                speechOn = true;
            }
            else if (txt.IndexOf("doble click") >= 0)
            {
                Console.WriteLine("doble click realizado");
                speechOn = true;
            }
            else if (txt.IndexOf("click") >= 0)
            {
                Console.WriteLine("click realizado");
                speechOn = true;
            }
            if (speechOn == false) return;
        } // sre_SpeechRecognized
    } // Program
} // ns
