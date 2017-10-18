using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.rdlc.entities.PImpresion
{
    public class EstadoDeCuenta
    {
        public string FechaVencimiento { get; set; }
        public decimal Importe { get; set; }
        public decimal SaldoCuota { get; set; }
        public string EndosoNro { get; set; }
    }
}