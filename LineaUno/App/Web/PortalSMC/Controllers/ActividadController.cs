using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using LineaUno.App.Web.Bus.PortalSMC;
using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using LineaUno.App.Web.Bus.PortalSMC.Model.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace PortalSMC.Controllers
{
    public class ActividadController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ActividadController(IConfiguration _configuration, IHostingEnvironment environment)
        {
            this.configuration = _configuration;
            _hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            return View("CargaExcel");
        }

        public IActionResult BusquedaCabecera()
        {
            return View("ListadoCabecereAcciones");
        }

        public IActionResult DetallesCargaAcciones()
        {
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["numCarga"]))
                ViewData["numeroCarga"] = HttpContext.Request.Query["numCarga"];
            return View("DetallesCargaAcciones");
        }

        public IActionResult EdicionAccion(List<MCDetCargaForDosBusquedaEdicionResponse> detCargaActividadBusquedaEdicion)
        {
            return View(detCargaActividadBusquedaEdicion);
        }

        public IActionResult PedidosTrabajo()
        {
            ViewData["Categorias"] = ListarCategoriasInconveniente();
            return View();
        }

        public IActionResult Notificaciones()
        {
            return View("Notificaciones");
        }

        //public IActionResult PedidosTrabajo()
        //{
            
        //}

        public async Task<IActionResult> CargaExcel(McmaeCargaForDosRequest maeCargaActividadRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    maeCargaActividadRequest.UsuarioCreacion = "QUALIS";
                    maeCargaActividadRequest.TerminalCreacion = "SERVER";

                    var maeCargaActividadBus = new McmaeCargaForDosBus();
                    string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;
                    var maeCargaActividadResponse = await maeCargaActividadBus.CargarExcel(maeCargaActividadRequest, baseUrl);
                    if (maeCargaActividadResponse != null)
                    {
                        if (maeCargaActividadResponse.IsSuccessStatusCode)
                        {
                            var carga = JsonConvert.DeserializeObject<McmaeCargaForDosResponse>(maeCargaActividadResponse.Content.ReadAsStringAsync().Result);
                            ViewBag.Mensaje = carga.Mensaje;
                            return View();
                        }
                        else
                        {
                            switch (maeCargaActividadResponse.StatusCode)
                            {
                                case HttpStatusCode.InternalServerError:
                                    var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(maeCargaActividadResponse.Content.ReadAsStringAsync().Result);
                                    ModelState.AddModelError("ErrorMensaje", mensajeErrorInterno.Mensaje);
                                    Console.Write("Error");
                                    break;
                                case HttpStatusCode.UnprocessableEntity:
                                    var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(maeCargaActividadResponse.Content.ReadAsStringAsync().Result);
                                    ModelState.AddModelError("ErrorMensaje", mensajeEntidadImprosesable.Mensaje);
                                    Console.Write("Error");
                                    break;
                                default:
                                    ModelState.AddModelError("ErrorMensaje", "Error interno en la aplicación web.");
                                    Console.Write("Error");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorMensaje", "Error de comunicación con el servidor.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorMensaje", "Error interno en la web.");
                Console.Write(ex.Message);
            }
            return View(maeCargaActividadRequest);
        }

        public IActionResult Gantt()
        {
            return View();
        }

        public async Task<IActionResult> ListadoPedidoTrabajoGantt(string pt, string desc, string fi, string fn)
        {
            try
            {
                GanttParRequest gpr = new GanttParRequest
                {
                    PT = pt,
                    Descripcion = desc,
                    FeIni = fi,
                    FeFin = fn
                };
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = await new PedidoTrabajoBus().Listado_Gantt(baseUrl, gpr);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.IsSuccessStatusCode)
                    {
                        var listadoPT = JsonConvert.DeserializeObject<List<PedidoTrabajoResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);
                        return Json(new { data = listadoPT });

                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error");
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> ListadoRecursoGantt(string pt, string desc, string fi, string fn)
        {
            try
            {
                GanttParRequest gpr = new GanttParRequest
                {
                    PT = pt,
                    Descripcion = desc,
                    FeIni = fi,
                    FeFin = fn
                };
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = await new RecursoBus().Listado_Gantt(baseUrl, gpr);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.IsSuccessStatusCode)
                    {
                        var listadoRec = JsonConvert.DeserializeObject<List<RecursoResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);
                        return Json(new { data = listadoRec });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error");
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> ListadoCabecera(DataSourceLoadOptionsBase loadOptions)
        {
            try
            {
                var maeCargaActividadBus = new McmaeCargaForDosBus();

                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var maeCargaActividadRequest = new McmaeCargaForDosListadoRequest { Activo = true };

                var maeCargaActividadListadoResponse = await maeCargaActividadBus.BusquedaCabecera(maeCargaActividadRequest, baseUrl);

                if (maeCargaActividadListadoResponse != null)
                {
                    if (maeCargaActividadListadoResponse.IsSuccessStatusCode)
                    {
                        //var  maeCargaActividadListado = JsonConvert.DeserializeObject<MCMaeCargaForDosListadoResponse>(jsonResponse);
                        //ICollection<PedidoTrabajoBResponse> listadoPT = JsonConvert.DeserializeObject<ICollection<PedidoTrabajoBResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);

                        ICollection<MCMaeCargaForDosListadoResponse> maeCargaActividadListado = JsonConvert.DeserializeObject<ICollection<MCMaeCargaForDosListadoResponse>>(maeCargaActividadListadoResponse.Content.ReadAsStringAsync().Result);

                        return  NewtonsoftJson(maeCargaActividadListado);
                    }
                    else
                    {
                        switch (maeCargaActividadListadoResponse.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(maeCargaActividadListadoResponse.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(maeCargaActividadListadoResponse.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            default:
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    Console.Write("Error");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error");
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> BusquedaDetallesCarga(int numCarga)
        {
            try
            {

                MCDetCargaForDosListadoRequest detCargaActividadListadoRequest = new MCDetCargaForDosListadoRequest
                {
                    numCarga = numCarga
                };
                var detCargaActividadBus = new MCDetCargaForDosBus();

                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await detCargaActividadBus.BusquedaDetalles(detCargaActividadListadoRequest, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ICollection<MCDetCargaForDosListadoResponse> detCargaActividadListado = JsonConvert.DeserializeObject<ICollection<MCDetCargaForDosListadoResponse>>(response.Content.ReadAsStringAsync().Result);
                        return NewtonsoftJson(detCargaActividadListado);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            default:
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    Console.Write("Error");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error");
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> BusquedaEdicionDetalle(MCDetCargaForDosBusquedaEdicionRequest request)
        {
            try
            {
                var detCargaActividadBus = new MCDetCargaForDosBus();


                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await detCargaActividadBus.BusquedaEdicionDetalle(request, baseUrl);

                ViewData["ListaPrioridades"] = ListarValores_x_ICodParametro();
                ViewData["Trabajadores"] = ListarTrabajadores_Carga(request.NumCarga, request.CodigoDetalle);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //string jsonResponse = await response.Content.ReadAsStringAsync();

                        //var detCargaActividadEdicion = JsonConvert.DeserializeObject<List<MCDetCargaForDosBusquedaEdicionResponse>>(jsonResponse);

                        var detCargaActividadEdicion = JsonConvert.DeserializeObject<MCDetCargaForDosBusquedaEdicionResponse>(response.Content.ReadAsStringAsync().Result);

                        return View("EdicionAccion", detCargaActividadEdicion);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            default:
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    Console.Write("Error");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error");
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> EdicionDetalle(MCDetCargaForDosEdicionRequest request)
        {
            try
            {
                var detCargaActividadBus = new MCDetCargaForDosBus();

                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var IP = Request.HttpContext.Connection.RemoteIpAddress;
                string compName = DetermineCompName(IP.ToString());

                if (request.Ruc == null)
                {
                    request.Ruc = " ";
                }


                request.NombreTerminal = compName;
                var response = await detCargaActividadBus.EdicionDetalle(request, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var detCargaActividadEdicion = JsonConvert.DeserializeObject<MCDetCargaForDosEdicionResponse>(response.Content.ReadAsStringAsync().Result);

                        if (detCargaActividadEdicion.Exito)
                        {
                            ViewBag.MensajeExito = detCargaActividadEdicion.Mensaje;
                        }
                        else
                        {
                            ViewBag.MensajeError = detCargaActividadEdicion.Mensaje;
                        }

                        var detCargaActividadBusquedaEdicion = new MCDetCargaForDosBusquedaEdicionResponse
                        {
                            NumCarga = request.NumCarga,
                            CodigoDetalle = request.CodigoDetalle,
                            Ruc = request.Ruc,
                            RazonSocial = request.RazonSocial,
                            ZonaEspecifica = request.ZonaEspecifica,
                            Prioridad = request.Prioridad,
                            PermisoInt = request.PermisoInt,
                            Trabajadores = request.Trabajadores
                        };
                        ViewData["ListaPrioridades"] = ListarValores_x_ICodParametro();
                        ViewData["Trabajadores"] = ListarTrabajadores_Carga(request.NumCarga, request.CodigoDetalle);
                        return View("EdicionAccion", detCargaActividadBusquedaEdicion);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            default:
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    Console.Write("Error");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error");
            }
            return Content("Ups! Un error ocurrio");
        }

        //CÓDIGO CREATIVIDAPPS
        public async Task<ActionResult> BusquedaPedidosTrabajo_Bandeja(DataSourceLoadOptionsBase loadOptions)
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = await new PedidoTrabajoBus().ListaPedidoTrabajoB(baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.IsSuccessStatusCode)
                    {
                        ICollection<PedidoTrabajoBResponse> listadoPT = JsonConvert.DeserializeObject<ICollection<PedidoTrabajoBResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);
                        //return Json(new { data = listadoPT });
                        //return NewtonsoftJson(await DataSourceLoader.LoadAsync(listadoPT, loadOptions));
                        return NewtonsoftJson(listadoPT);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        ActionResult NewtonsoftJson(object obj, int statusCode = 200)
        {
            Response.StatusCode = statusCode;
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        public async Task<IActionResult> IniciarPedidoTrabajo(PedidosTrabajo request)
        {
            
            try
            {
                var IP = Request.HttpContext.Connection.RemoteIpAddress;
                string compName = DetermineCompName(IP.ToString());

                var req = new MCMovEstadoPTRequest
                {
                    iNumIdPT = request.MovimientoEstadoPT.iNumIdPT,
                    iNumDetPT = request.MovimientoEstadoPT.iNumDetPT,
                    vCodUsuarioCreacion = request.MovimientoEstadoPT.vCodUsuarioCreacion,
                    iNumDetEstPT = request.MovimientoEstadoPT.iNumDetEstPT,
                    Terminal = compName,
                    Opcion = request.MovimientoEstadoPT.Opcion
                };

                var pedido = new PedidoTrabajoBus();

                //request.Terminal = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await pedido.IniciarPedidoTrabajo(req, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var carga = JsonConvert.DeserializeObject<McmaeCargaForDosResponse>(response.Content.ReadAsStringAsync().Result);
                        if(carga.Exito == true)
                            ViewBag.MensajeExito = carga.Mensaje;
                        else
                            ViewBag.MensajeError = carga.Mensaje;
                        ViewData["Categorias"] = ListarCategoriasInconveniente();
                        return View("PedidosTrabajo");
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeErrorInterno.Mensaje);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeEntidadImprosesable.Mensaje);
                                Console.Write("Error");
                                break;
                            default:
                                ModelState.AddModelError("ErrorMensaje", "Error interno en la aplicación web.");
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorMensaje", "Error de comunicación con el servidor.");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<ActionResult> ObtenerListaMotivos_x_Categoria(int idCategoria)
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = await new ParametroValorBus().ListadoMotivos_x_Categoria(idCategoria, baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.IsSuccessStatusCode)
                    {
                        ICollection<ParametroValorResponse> listadoValores = JsonConvert.DeserializeObject<ICollection<ParametroValorResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);
                        //return Json(new { data = listadoPT });
                        //return NewtonsoftJson(await DataSourceLoader.LoadAsync(listadoPT, loadOptions));
                        return NewtonsoftJson(listadoValores);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> RegistrarInconvPT(PedidosTrabajo request)
        {
            List<string> paths = new List<string>();
            //var a = Dns.GetHostByAddress(Request.Host.Host .ServerVariables.Item("REMOTE_HOST")).HostName;
            try
            {

                var IP = Request.HttpContext.Connection.RemoteIpAddress;
                string compName = DetermineCompName(IP.ToString());

                var req = new MCMovBanPTRequest
                {
                    iNumIdPT = request.MovimientoBanPT.iNumIdPT,
                    iNumDetPT = request.MovimientoBanPT.iNumDetPT,
                    vCodUsuarioCreacion = request.MovimientoBanPT.vCodUsuarioCreacion,
                    ICodTraResponsable = request.MovimientoBanPT.ICodTraResponsable,
                    IdCategoria = request.MovimientoBanPT.IdCategoria,
                    VTipInconveniente = request.MovimientoBanPT.VTipInconveniente,
                    VDescInconveniente = request.MovimientoBanPT.VDescInconveniente,
                    Mensaje = request.MovimientoBanPT.Mensaje,
                    Terminal = compName,
                    Email = request.MovimientoBanPT.Email
                };

                if (Request.Form.Files.Count > 0)
                {
                    //req.Adjuntos = Request.Form.Files;
                    foreach (var file in Request.Form.Files)
                    {
                        var ruta = SaveFile(file, request.MovimientoBanPT.PedidoTrabajo, "AdjuntosEnvio");
                        paths.Add(ruta);
                    }
                    req.Rutas = paths;
                }

                var pedido = new PedidoTrabajoBus();

                //request.Terminal = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await pedido.RegistrarInconvenientePT(req, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var carga = JsonConvert.DeserializeObject<McmaeCargaForDosResponse>(response.Content.ReadAsStringAsync().Result);
                        if (carga.Exito == true)
                            ViewBag.MensajeExito = carga.Mensaje;
                        else
                            ViewBag.MensajeError = carga.Mensaje;
                        ViewData["Categorias"] = ListarCategoriasInconveniente();
                        return View("PedidosTrabajo");
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeErrorInterno.Mensaje);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeEntidadImprosesable.Mensaje);
                                Console.Write("Error");
                                break;
                            default:
                                ModelState.AddModelError("ErrorMensaje", "Error interno en la aplicación web.");
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorMensaje", "Error de comunicación con el servidor.");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        public static string DetermineCompName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First();
        }

        //NOTIFICACIONES
        public async Task<ActionResult> BusquedaNotificaciones_Bandeja(DataSourceLoadOptionsBase loadOptions)
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = await new NotificacionBus().ListaNotificacionB(baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.IsSuccessStatusCode)
                    {
                        ICollection<NotificacionBanResponse> listadoNot = JsonConvert.DeserializeObject<ICollection<NotificacionBanResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);
                        //return Json(new { data = listadoPT });
                        //return NewtonsoftJson(await DataSourceLoader.LoadAsync(listadoPT, loadOptions));
                        return NewtonsoftJson(listadoNot);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> ResponderInconvPT(PedidosTrabajo request)
        {
            List<string> paths = new List<string>();
            //var a = Dns.GetHostByAddress(Request.Host.Host .ServerVariables.Item("REMOTE_HOST")).HostName;
            try
            {

                var IP = Request.HttpContext.Connection.RemoteIpAddress;
                string compName = DetermineCompName(IP.ToString());

                var req = new MCMovBanPTRequest
                {
                    iNumIdPT = request.MovimientoBanPT.iNumIdPT,
                    iNumDetPT = request.MovimientoBanPT.iNumDetPT,
                    vCodUsuarioCreacion = request.MovimientoBanPT.vCodUsuarioCreacion,
                    ICodTraResponsable = request.MovimientoBanPT.ICodTraResponsable,
                    VTipInconveniente = request.MovimientoBanPT.VTipInconveniente,
                    Mensaje = request.MovimientoBanPT.Mensaje,
                    Terminal = compName,
                    Email = request.MovimientoBanPT.Email
                };

                if(Request.Form.Files.Count > 0)
                {
                    //req.Adjuntos = Request.Form.Files;
                    foreach (var file in Request.Form.Files)
                    {
                        var ruta = SaveFile(file, request.MovimientoBanPT.PedidoTrabajo, "AdjuntosRpta");
                        paths.Add(ruta);
                    }
                    req.Rutas = paths;
                }

                var pedido = new NotificacionBus();

                //request.Terminal = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await pedido.ResponderInconvenientePT(req, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var carga = JsonConvert.DeserializeObject<McmaeCargaForDosResponse>(response.Content.ReadAsStringAsync().Result);
                        if (carga.Exito == true)
                            ViewBag.MensajeExito = carga.Mensaje;
                        else
                            ViewBag.MensajeError = carga.Mensaje;
                        ViewData["Categorias"] = ListarCategoriasInconveniente();
                        return View("Notificaciones");
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeErrorInterno.Mensaje);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeEntidadImprosesable.Mensaje);
                                Console.Write("Error");
                                break;
                            default:
                                ModelState.AddModelError("ErrorMensaje", "Error interno en la aplicación web.");
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorMensaje", "Error de comunicación con el servidor.");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        public string SaveFile(IFormFile file, string PT, string folderName)
        {

            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
                var fileName = file.FileName;
                var fullPath = Path.Combine(path, PT);
                var dbPath = Path.Combine(fullPath, fileName);
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                using (var stream = new FileStream(dbPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }



                return dbPath;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<IActionResult> FinalizarPedidoTrabajo(PedidosTrabajo request)
        {

            try
            {
                var IP = Request.HttpContext.Connection.RemoteIpAddress;
                string compName = DetermineCompName(IP.ToString());

                var req = new MCMovEstadoPTRequest
                {
                    iNumIdPT = request.MovimientoEstadoPT.iNumIdPT,
                    iNumDetPT = request.MovimientoEstadoPT.iNumDetPT,
                    vCodUsuarioCreacion = request.MovimientoEstadoPT.vCodUsuarioCreacion,
                    iNumDetEstPT = request.MovimientoEstadoPT.iNumDetEstPT,
                    Terminal = compName,
                    Opcion = request.MovimientoEstadoPT.Opcion
                };

                var pedido = new PedidoTrabajoBus();

                //request.Terminal = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await pedido.FinalizarPedidoTrabajo(req, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var carga = JsonConvert.DeserializeObject<McmaeCargaForDosResponse>(response.Content.ReadAsStringAsync().Result);
                        if (carga.Exito == true)
                            ViewBag.MensajeExito = carga.Mensaje;
                        else
                            ViewBag.MensajeError = carga.Mensaje;
                        ViewData["Categorias"] = ListarCategoriasInconveniente();
                        return View("PedidosTrabajo");
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeErrorInterno.Mensaje);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeEntidadImprosesable.Mensaje);
                                Console.Write("Error");
                                break;
                            default:
                                ModelState.AddModelError("ErrorMensaje", "Error interno en la aplicación web.");
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorMensaje", "Error de comunicación con el servidor.");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        // Categorías de inconveniente
        public IEnumerable<ParametroValorResponse> ListarCategoriasInconveniente()
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = new ParametroValorBus().ListadoCategoriasInconvenientes(baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.Result.IsSuccessStatusCode)
                    {
                        IEnumerable<ParametroValorResponse> listadoValores = JsonConvert.DeserializeObject<IEnumerable<ParametroValorResponse>>(listadoResponseMessage.Result.Content.ReadAsStringAsync().Result);
                        return listadoValores;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return null;
        }

        public async Task<ActionResult> ObtenerListaMotivos_Inconveniente(int PT, int detPT)
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = await new NotificacionBus().ObtenerListaMotivos_Inconveniente(PT, detPT, baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.IsSuccessStatusCode)
                    {
                        ICollection<MotivosIncResponse> listadoValores = JsonConvert.DeserializeObject<ICollection<MotivosIncResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);
                        //return Json(new { data = listadoPT });
                        //return NewtonsoftJson(await DataSourceLoader.LoadAsync(listadoPT, loadOptions));
                        return NewtonsoftJson(listadoValores);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        public IEnumerable<ParametroValorResponse> ListarValores_x_ICodParametro()
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = new ParametroValorBus().ListarValores_x_ICodParametro(6, baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.Result.IsSuccessStatusCode)
                    {
                        IEnumerable<ParametroValorResponse> listadoValores = JsonConvert.DeserializeObject<IEnumerable<ParametroValorResponse>>(listadoResponseMessage.Result.Content.ReadAsStringAsync().Result);
                        return listadoValores;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return null;
        }

        public async Task<ActionResult> ListarTrabajadores_Rec()
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage = await new TrabajadorBus().ListarTrabajadores_Rec(baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.IsSuccessStatusCode)
                    {
                        ICollection<MCTrabajadorResponse> listadoValores = JsonConvert.DeserializeObject<ICollection<MCTrabajadorResponse>>(listadoResponseMessage.Content.ReadAsStringAsync().Result);
                        //return Json(new { data = listadoPT });
                        //return NewtonsoftJson(await DataSourceLoader.LoadAsync(listadoPT, loadOptions));
                        return NewtonsoftJson(listadoValores);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }

        public ResStringResponse ListarTrabajadores_Carga(int numCarga, int numDetCarga)
        {
            try
            {
                string baseUrl = configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var listadoResponseMessage =  new MCDetCargaForDosBus().ListarTrabajadores_Carga(numCarga, numDetCarga, baseUrl);

                if (listadoResponseMessage != null)
                {
                    if (listadoResponseMessage.Result.IsSuccessStatusCode)
                    {
                        ResStringResponse respuesta = JsonConvert.DeserializeObject<ResStringResponse>(listadoResponseMessage.Result.Content.ReadAsStringAsync().Result);
                        return respuesta;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return null;
        }

        public async Task<IActionResult> ProcesarPlantilla(McParametrosProcessPlantillaRequest request)
        {

            try
            {
                var IP = Request.HttpContext.Connection.RemoteIpAddress;
                string compName = DetermineCompName(IP.ToString());

                request.Computador = compName;

                var carga = new McmaeCargaForDosBus();

                //request.Terminal = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await carga.ProcesarPlantilla(request, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var resultado = JsonConvert.DeserializeObject<McmaeCargaForDosResponse>(response.Content.ReadAsStringAsync().Result);
                        if (resultado.Exito == true)
                            ViewBag.MensajeExito = resultado.Mensaje;
                        else
                            ViewBag.MensajeError = resultado.Mensaje;
                        //ViewData["Categorias"] = ListarCategoriasInconveniente();
                        return View("ListadoCabecereAcciones");
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeErrorInterno.Mensaje);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(response.Content.ReadAsStringAsync().Result);
                                ModelState.AddModelError("ErrorMensaje", mensajeEntidadImprosesable.Mensaje);
                                Console.Write("Error");
                                break;
                            default:
                                ModelState.AddModelError("ErrorMensaje", "Error interno en la aplicación web.");
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorMensaje", "Error de comunicación con el servidor.");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.Message);
            }
            return Content("Ups! Un error ocurrio");
        }
    }
}