using System.ComponentModel.DataAnnotations;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Request
{
    public class UsuarioAutenticacionRequest
    {
        [StringLength(100, ErrorMessage = "Usuario debe tener hasta un máximo de 100 caracteres.")]
        [Required(ErrorMessage = "El campo usuario es requerido.")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [StringLength(100, ErrorMessage = "Contraseña debe tener hasta un máximo de 100 caracteres.")]
        [Required(ErrorMessage = "El campo contraseña es requerido.")]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

      
        public string ErrorMensaje { get; set; }
    }
}
