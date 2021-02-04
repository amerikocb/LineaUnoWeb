using System;
using System.Collections.Generic;
using System.Text;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Response
{
    public class NotificacionBanResponse
    {
        public int IdPT { get; set; }
        public int IdDetPT { get; set; }
        public string De { get; set; }
        public string PT { get; set; }
        public string Tipo { get; set; }
        public DateTime Recibido { get; set; }
        public string Estado { get; set; }
        public string Motivos { get; set; }
        public string Mensaje { get; set; }
    }
}
