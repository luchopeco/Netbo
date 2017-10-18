using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.CargaPuntosConsulta
{
    public class TotalesModels
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public int TotalPuntosCargados { get; set; }

        public decimal MontoEnPesosTotal { get; set; }
    }
}