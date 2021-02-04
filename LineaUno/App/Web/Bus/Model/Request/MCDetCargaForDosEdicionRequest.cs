using System;
using System.Collections.Generic;
using System.Text;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Request
{
    public class MCDetCargaForDosEdicionRequest
    {
        public int NumCarga { get; set; }
        public int CodigoDetalle { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string ZonaEspecifica { get; set; }
        public string Prioridad { get; set; }
        public string PermisoInt { get; set; }
        public string Trabajadores { get; set; }
        public string NombreTerminal { get; set; }
        public string Usuario { get; set; }

    }
}
