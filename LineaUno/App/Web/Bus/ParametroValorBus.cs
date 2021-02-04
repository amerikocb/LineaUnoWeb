using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class ParametroValorBus
    {
        public async Task<HttpResponseMessage> ListadoMotivos_x_Categoria(int idCategoria, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new StringContent(JsonConvert.SerializeObject(idCategoria), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(baseUrl + "listar-motivos-x-categoria/" + idCategoria, null);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> ListadoCategoriasInconvenientes(string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.PostAsync(baseUrl + "listar-cat-inconveniente", null);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> ListarValores_x_ICodParametro(int iCodParametro, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.PostAsync(baseUrl + "listar-valores-codparametro/" + iCodParametro, null);
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
