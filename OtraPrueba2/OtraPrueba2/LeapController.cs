using Leap;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace OtraPrueba2
{
    public class LeapController
    {

        private byte[] imagedata = new byte[1];
        private Controller controller = new Controller();
        public LeapController()
        {
            controller.FrameReady += newFrameHandler;
        }

        enum Yankenpo { Piedra, Papel, Tijera, Nada }
        Yankenpo ObtenerElemento(Hand mano)
        {
            Yankenpo elemento;
            if (mano.Fingers[0].IsExtended && mano.Fingers[1].IsExtended && mano.Fingers[2].IsExtended && mano.Fingers[3].IsExtended && mano.Fingers[4].IsExtended)
            {
                elemento = Yankenpo.Papel;
            }
            else if (!mano.Fingers[0].IsExtended && mano.Fingers[1].IsExtended && mano.Fingers[2].IsExtended && !mano.Fingers[3].IsExtended && !mano.Fingers[4].IsExtended)
            {
                elemento = Yankenpo.Tijera;
            }
            else if (!mano.Fingers[0].IsExtended && !mano.Fingers[1].IsExtended && !mano.Fingers[2].IsExtended && !mano.Fingers[3].IsExtended && !mano.Fingers[4].IsExtended)
            {
                elemento = Yankenpo.Piedra;
            }
            else
            {
                elemento = Yankenpo.Nada;
            }
            return elemento;
        }

        void newFrameHandler(object sender, FrameEventArgs eventArgs)
        {
            Frame frame = eventArgs.frame;
            int extendidos = 0;
            if (frame.Hands.Count > 0)
            {
                // Console.WriteLine("Hay manos");
                Hand mano = frame.Hands[0];
                // foreach (Finger f in mano.Fingers)
                // {
                //     if (f.IsExtended)
                //         extendidos++;
                // }
                // if (mano.Fingers[1].IsExtended) {
                //     Console.WriteLine("No señalando");
                // }
                // else {
                //     Console.WriteLine("No señalando");
                // }
                //Console.WriteLine(extendidos.ToString());
                //Console.WriteLine(ObtenerElemento(mano).ToString());

                Vector pos = mano.Fingers[1].StabilizedTipPosition;
                MouseHook.SetCursorPos((int)pos.x * 7, (int)pos.y * (-3) + 500);
                //this.Cursor = new Cursor(Cursor.Current.Handle);
                //Cursor.Position = new Point((int)pos.x * 7, (int)pos.y * (-3) + 500);
                //Cursor.Clip = new Rectangle(this.Location, this.Size);
            }
            else
            {
                //Console.WriteLine("Nada");
            }
            controller.RequestImages(frame.Id, Leap.Image.ImageType.DEFAULT, imagedata);
        }
    }
}