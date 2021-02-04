using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class MCDetCargaForDosBus
    {
        public async Task<HttpResponseMessage> BusquedaDetalles(MCDetCargaForDosListadoRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.GetAsync(baseUrl + "carga-actividad/"+ request.numCarga + "/detalles");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> BusquedaEdicionDetalle(MCDetCargaForDosBusquedaEdicionRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                       response = await client.GetAsync(baseUrl + "actividad/" + request.NumCarga +","+request.CodigoDetalle);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> EdicionDetalle(MCDetCargaForDosEdicionRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    response = await client.PutAsync(baseUrl + "actividad/edicion", data);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> ListarTrabajadores_Carga(int numCarga, int numDetCarga, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    //var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(baseUrl + "busqueda-trabajadores/" + numCarga + "/" + numDetCarga, null);
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
