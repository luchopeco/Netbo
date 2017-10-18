using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.rdlc.entities.PImpresion
{
    public class IncisoOr
    {
        public int IdInciso { get; set; }
        public int IncisoNro { get; set; }
        public string BienAsegurado { get; set; }
        public string DescripcionCobertura { get; set; }
        public decimal SumaAsegurada { get; set; }
    }
}