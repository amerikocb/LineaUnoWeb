using System;
using System.Collections.Generic;
using System.Text;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Response
{
    public class PedidoTrabajoBResponse
    {
        public int IdPT { get; set; }
        public int IdDetPT { get; set; }
        public string EstadoPT { get; set; }
        public string PT { get; set; }
        public string Descripcion { get; set; }
        public string Zona { get; set; }
        public DateTime FechaHora { get; set; }
        public int CodigoTrabajador { get; set; }
        public string Trabajador { get; set; }
        public string Email { get; set; }
    }
}
