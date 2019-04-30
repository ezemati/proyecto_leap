using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace servicioPaginaNoMicrosoft
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            /* APRETAR F12
            this.serviceInstaller1.Description --> Descripcion del servicio, se muestra en la ventana de los servicios
            this.serviceInstaller1.DisplayName --> Nombre que muestra la ventana de servicios para este servicio
            this.serviceInstaller1.ServiceName --> No se si se puede cambiar esto
            */
        }
    }
}
