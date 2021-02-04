using Microsoft.AspNetCore.Mvc;
namespace LineaUno.App.Web.Bus.PortalSMC.Model.Request
{
    public class GanttParRequest
    {
        public string PT { get; set; }
        public string Descripcion { get; set; }
        public string FeIni { get; set; }
        public string FeFin { get; set; }
    }
}
