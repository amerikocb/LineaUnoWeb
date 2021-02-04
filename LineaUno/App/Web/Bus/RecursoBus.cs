using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class RecursoBus
    {
        public async Task<HttpResponseMessage> Listado(string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.PostAsync(baseUrl + "listar-recurso", null);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> Listado_Gantt(string baseUrl, GanttParRequest request)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(baseUrl + "listar-rec-pt-gantt", data);
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