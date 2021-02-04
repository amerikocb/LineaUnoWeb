using System;
using System.ComponentModel.DataAnnotations;

namespace LineaUno.App.Web.Bus.PortalSMC.Model.Request
{
    public class MCMovEstadoPTRequest
    {
        public int iNumIdPT { get; set; }
        public int iNumDetPT { get; set; }
        public int iNumDetEstPT { get; set; }
        public string vCodUsuarioCreacion { get; set; }
        public string Terminal { get; set; }
        public string Opcion { get; set; }
    }
}
