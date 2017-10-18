using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.CanjePuntosConsultas
{
    public class TotalesPorComplejoModels
    {
        public int IdComplejo { get; set; }
        public string Complejo { get; set; }
        public decimal TotalPuntosCanjeados { get; set; }
        public decimal TotalCanjesRealizados { get; set; }

        public decimal PorcentajePuntosDelTotal { get; set; }
        public decimal PorcentajeCanjesDelTotal { get; set; }

        public decimal TotalPuntosCargados { get; set; }
        public decimal PorcentajePuntosCargadosDelTotal { get; set; }

        public List<TotalesPorComplejoPremiosModels> ListPremios { get; set; }
    }

    public class TotalesPorComplejoPremiosModels
    {
        public string Premio { get; set; }
        public int CantidadCanjeada { get; set; }
    }
}