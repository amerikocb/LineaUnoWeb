
namespace LineaUno.App.Web.Bus.PortalSMC.Model.Response
{
    public class PedidoTrabajoResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Descripcion { get; set; }
        public string Start_date { get; set; }
        public decimal Duration { get; set; }
        public decimal TotalHoras { get; set; }
        public bool Inconveniente { get; set; }
        public int Operarios { get; set; }
        public string Especialidad { get; set; }
        public string Actividad { get; set; }
        public string Activo { get; set; }
        public string Area { get; set; }
        public string Zona { get; set; }
        public string Tipo { get; set; }
        public bool Open { get; set; }
        public string Render { get; set; }
        public int? Parent { get; set; }
        public bool Urgencia { get; set; }
    }
}