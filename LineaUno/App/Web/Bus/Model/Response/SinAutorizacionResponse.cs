using System;
using System.Collections.Generic;
using System.Text;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Response
{
    public class SinAutorizacionResponse
    {
        public string Mensaje { get; set; }
        public int StatusCode { get; set; }
        public string Error { get; set; }
    }
}
