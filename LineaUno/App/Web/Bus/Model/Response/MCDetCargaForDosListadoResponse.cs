using System;
using System.Collections.Generic;
using System.Text;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Response
{
    public class MCDetCargaForDosListadoResponse
    {
        public int NumCarga { get; set; }
        public int NumDetCarga { get; set; }
        public string NumPT { get; set; }
        public string ParFecha { get; set; }
        public string HorInicio { get; set; }
        public string HorFin { get; set; }
        public string AreResponsable { get; set; }
        public string Ruc { get; set; }
        public string RazSocial { get; set; }
        public string Zona { get; set; }
        public string SecLinea { get; set; }
        public string ZonEspecifica { get; set; }
        public string DesActividad { get; set; }
        public string TipActividad { get; set; }
        public string Prioridad { get; set; }
        public string PerIntervencion { get; set; }
        public string RecMedios { get; set; }
        public string SupResConcar { get; set; }
        public string CelSupResponsable { get; set; }
        public string TEResConcar { get; set; }
        public string CelTEResponsable { get; set; }
        public string ResTercero { get; set; }
        public string CelTercero { get; set; }
        public string CodUsuCreacion { get; set; }
        public string NomTerCreacion { get; set; }
    }
}
