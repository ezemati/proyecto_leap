using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace servicioPaginaNoMicrosoft
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Service1 service1 = new Service1(args);
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1(args)
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
