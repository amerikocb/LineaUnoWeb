using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Request
{
    public class McmaeCargaForDosRequest
    {
        public string UsuarioCreacion { get; set; }
        public string TerminalCreacion { get; set; }

        [Required(ErrorMessage = "Cargar un archivo es requerido.")]
        [Display(Name = "Archivo")]
        public IFormFile File { get; set; }

        public string ErrorMensaje { get; set; }
    }
}
