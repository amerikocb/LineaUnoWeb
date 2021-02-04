using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class MCTabOTBus
    {
     
        public async Task<HttpResponseMessage> BusquedaListadoOT(MCTabOTListadoRequest request, string baseUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.GetAsync(baseUrl + "busqueda-listado/activo/"+ request.Activo);

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
