using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LineaUno.App.Web.Bus.PortalSMC;
using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using LineaUno.App.Web.Bus.PortalSMC.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace PortalSMC.Controllers
{
    public class GestionController : Controller
    {
        private readonly IConfiguration configuration;

        public GestionController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public IActionResult Index()
        {
            //return View("CargaExcel");
            return View();
        }

        //public IActionResult ListadoCabecereAcciones(List<MaeCargaActividadListadoResponse> maeCargaActividadListado)
        //{
        //    return View(maeCargaActividadListado);
        //}

        //public IActionResult DetallesCargaAcciones(List<DetCargaActividadListadoResponse> detCargaActividadListado)
        //{
        //    return View(detCargaActividadListado);
        //}

        //public IActionResult EdicionAccion(List<DetCargaActividadBusquedaEdicionResponse> detCargaActividadBusquedaEdicion)
        //{
        //    return View(detCargaActividadBusquedaEdicion);
        //}

   
        public async Task<IActionResult> BusquedaListadoOT()
        {
            try
            {
                var mcTabOTBus = new MCTabOTBus();

                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var mcTabOTRequest = new MCTabOTListadoRequest { Activo = true };

                var mcTabOTListadoResponse = await mcTabOTBus.BusquedaListadoOT(mcTabOTRequest, baseUrl);

                if (mcTabOTListadoResponse != null)
                {
                    if (mcTabOTListadoResponse.IsSuccessStatusCode)
                    {
                        var maeCargaActividadListado = JsonConvert.DeserializeObject<List<MCMaeCargaForDosListadoResponse>>(mcTabOTListadoResponse.Content.ReadAsStringAsync().Result);
                        return View("ListadoCabecereAcciones", maeCargaActividadListado);
                    }
                    else
                    {
                        switch (mcTabOTListadoResponse.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                var mensajeErrorInterno = JsonConvert.DeserializeObject<ErrorInternoResponse>(mcTabOTListadoResponse.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            case HttpStatusCode.UnprocessableEntity:
                                var mensajeEntidadImprosesable = JsonConvert.DeserializeObject<ErrorInternoResponse>(mcTabOTListadoResponse.Content.ReadAsStringAsync().Result);
                                Console.Write("Error");
                                break;
                            default:
                                Console.Write("Error");
                                break;
                        }
                    }
                }
                else {
                    Console.Write("Error");
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error");
            }
            return Content("Ups! Un error ocurrio");
        }

        public async Task<IActionResult> BusquedaDetalles(MCDetCargaForDosListadoRequest detCargaActividadListadoRequest)
        {
            try
            {
                var detCargaActividadBus = new MCDetCargaForDosBus();

                string baseUrl = this.configuration.GetSection("AppSettings").GetSection("Servicio").Value;

                var response = await detCargaActividadBus.BusquedaDetalles(detCargaActividadListadoRequest, baseUrl);

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var detCargaActividadListado = JsonConvert.DeserializeObject<List<MCDetCargaForDosListadoResponse>>(response.Content.ReadAsStringAsync().Result);
                        return View("DetallesCargaAcciones", detCargaActividadListado);
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

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
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
                        else {
                            ViewBag.MensajeError = detCargaActividadEdicion.Mensaje;
                        }

                        var detCargaActividadBusquedaEdicion = new MCDetCargaForDosBusquedaEdicionResponse
                        {
                            CodigoDetalle = request.CodigoDetalle,
                            Ruc = request.Ruc,
                            RazonSocial = request.RazonSocial,
                            ZonaEspecifica = request.ZonaEspecifica,
                            Prioridad = request.Prioridad,
                            PermisoInt = request.PermisoInt
                        };

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
    }
}