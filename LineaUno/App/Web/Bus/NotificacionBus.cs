using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class NotificacionBus
    {
        public async Task<HttpResponseMessage> ListaNotificacionB(string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.PostAsync(baseUrl + "listar-not-b", null);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> ResponderInconvenientePT(MCMovBanPTRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(baseUrl + "actividad/responder-inconveniente", data);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> ObtenerListaMotivos_Inconveniente(int PT, int detPT, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    //var data = new StringContent(JsonConvert.SerializeObject(PT), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(baseUrl + "listar-motivos-inc/" + PT.ToString() + "/" + detPT.ToString(), null);
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