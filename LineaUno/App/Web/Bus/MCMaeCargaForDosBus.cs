using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class McmaeCargaForDosBus
    {
        public async Task<HttpResponseMessage> CargarExcel(McmaeCargaForDosRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var x = request.File.OpenReadStream();
                    StreamContent streamContent = new StreamContent(x);
                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(streamContent, "file", request.File.FileName);

                    multipartContent.Add(new StringContent(request.UsuarioCreacion), "UsuarioCreacion");
                    multipartContent.Add(new StringContent(request.TerminalCreacion), "TerminalCreacion");

                    response = await client.PostAsync(baseUrl + "carga", multipartContent);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> BusquedaCabecera(McmaeCargaForDosListadoRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.GetAsync(baseUrl + "busqueda-actividades/activo/"+ request.Activo);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> ProcesarPlantilla(McParametrosProcessPlantillaRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(baseUrl + "carga/procesar-plantilla", data);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }
    }
}
