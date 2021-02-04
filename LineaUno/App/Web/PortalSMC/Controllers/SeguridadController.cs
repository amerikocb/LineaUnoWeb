using LineaUno.App.Web.Bus.PortalSMC;
using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using LineaUno.App.Web.Bus.PortalSMC.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace PortalSMC.Controllers
{
    public class SeguridadController : Controller
    {
        private readonly IConfiguration configuration;

        public SeguridadController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public async Task<IActionResult> Login(UsuarioAutenticacionRequest usuarioRequest)
        {
            HttpContext.Session.SetString("UserName1", "Usuario testing");
            return RedirectToAction("Index", "Home");
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var usuarioBus = new UsuarioBus();
            //        string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

            //        var usuarioResponse = await usuarioBus.ConsultaUsuario(usuarioRequest, baseUrl);
            //        if (usuarioResponse != null)
            //        {
            //            if (usuarioResponse.IsSuccessStatusCode)
            //            {

            //                string jsonResponse = await usuarioResponse.Content.ReadAsStringAsync();
            //                UsuarioAutenticacionResponse Response = JsonConvert.DeserializeObject<UsuarioAutenticacionResponse>(jsonResponse);
            //                HttpContext.Session.SetString("UserName1", Response.NombreCompleto);
            //                return RedirectToAction("Index", "Home");
            //            }
            //            else
            //            {
            //                switch (usuarioResponse.StatusCode)
            //                {
            //                    case HttpStatusCode.Unauthorized:
            //                        var mensajeSinAutorizacion = JsonConvert.DeserializeObject<SinAutorizacionResponse>(usuarioResponse.Content.ReadAsStringAsync().Result);
            //                        ModelState.AddModelError("ErrorMensaje", mensajeSinAutorizacion.Mensaje);
            //                        break;
            //                    case HttpStatusCode.InternalServerError:
            //                        var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(usuarioResponse.Content.ReadAsStringAsync().Result);
            //                        ModelState.AddModelError("ErrorMensaje", mensajeErrorInterno.Mensaje);
            //                        Console.Write("Error");
            //                        break;
            //                    default:
            //                        ModelState.AddModelError("ErrorMensaje", "Error interno en la aplicación web.");
            //                        Console.Write("Error");
            //                        break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            ModelState.AddModelError("ErrorMensaje", "Error de comunicación con el servidor.");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("ErrorMensaje", "Error interno en la web.");
            //    Console.Write(ex.Message);
            //}

            //return View(usuarioRequest);
        }
    }
}