using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using LineaUno.App.Web.Bus.PortalSMC.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class UsuarioBus
    {
        public async Task<HttpResponseMessage> ConsultaUsuario(UsuarioAutenticacionRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(baseUrl + "login", data);
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
