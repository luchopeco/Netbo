using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.rdlc.entities.PImpresion
{
    public class Inciso
    {
        public int IdInciso { get; set; }
        public int IncisoNro { get; set; }
        public string Patente { get; set; }
        public string Anio { get; set; }
        public string DescripcionCobertura { get; set; }
        public string FechaBaja { get; set; }
        public string DescripcionFranquicia { get; set; }
        public string BienAsegurado { get; set; }
        public decimal SumaAsegurada { get; set; }
        public string AcreedorPrendario { get; set; }
    }
}