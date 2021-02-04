using System;
using System.Collections.Generic;
using System.Text;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Response
{
    public class MCMaeCargaForDosListadoResponse
    {
        public int numCarga { get; set; }
        public string Estado { get; set; }
        public string TipCarga { get; set; }
        public string codUsuCreacion { get; set; }
        public DateTime sdFEcCreacion { get; set; }
    }
}
