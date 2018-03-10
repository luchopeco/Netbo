using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.CargaPuntosConsulta
{
    public class TotalesModels
    {
        public TotalesModels()
        {
            ListComplejos = new List<TotalPorComplejoModels>();
        }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public int TotalPuntosCargados { get; set; }

        public decimal MontoEnPesosTotal { get; set; }

        public int CantidadCargasRealizadas { get; set; }

        public List<TotalPorComplejoModels> ListComplejos { get; set; }
    }

    public class TotalPorComplejoModels
    {
        public string Complejo { get; set; }
        public int ComplejoId { get; set; }
        public int TotalPuntosCargados { get; set; }
        public decimal MontoEnPesosTotal { get; set; }
        public int CantidadCargasRealizadas { get; set; }

        public decimal PorcentajePuntosCargados { get; set; }
        public decimal PorcentajeMontoEnPesosCargado { get; set; }

        public decimal PorcentajeCantidadCargasRealizadas { get; set; }
    }
}