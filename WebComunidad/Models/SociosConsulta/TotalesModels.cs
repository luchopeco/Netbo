using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.SociosConsulta
{
    public class TotalesModels
    {
        public TotalesModels()
        {
            ListComplejos = new List<TotalPorComplejoModels>();
        }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int CantidadSocios { get; set; }
        public List<TotalPorComplejoModels> ListComplejos { get; set; }
    }

    public class TotalPorComplejoModels
    {
        public string Complejo { get; set; }
        public int ComplejoId { get; set; }
        public int CantidadSocios { get; set; }
        public decimal PorcentajeCantidadSocios { get; set; }
    }
}