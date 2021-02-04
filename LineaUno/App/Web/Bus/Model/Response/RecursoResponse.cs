
namespace LineaUno.App.Web.Bus.PortalSMC.Model.Response
{
    public class RecursoResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Start_date { get; set; }
        public decimal Duration { get; set; }
        public string Tipo { get; set; }
        public bool Inconveniente { get; set; }
        public bool Supervisor { get; set; }
        public bool Tecnico { get; set; }
        public bool Open { get; set; }
        public int Parent { get; set; }
        public bool Urgencia { get; set; }
    }
}