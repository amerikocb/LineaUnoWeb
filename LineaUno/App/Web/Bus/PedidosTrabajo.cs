using LineaUno.App.Web.Bus.PortalSMC.Model.Request;
using LineaUno.App.Web.Bus.PortalSMC.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LineaUno.App.Web.Bus.PortalSMC
{
    public class PedidosTrabajo
    {
        public MCMovEstadoPTRequest MovimientoEstadoPT { get; set; }
        public MCMovBanPTRequest MovimientoBanPT { get; set; }
        public virtual IEnumerable<ParametroValorResponse> ParametroValor { get; set; }
    }
}
