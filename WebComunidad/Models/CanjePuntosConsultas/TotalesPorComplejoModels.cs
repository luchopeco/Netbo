using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.CanjePuntosConsultas
{
    public class TotalesPorComplejoModels
    {
        public TotalesPorComplejoModels()
        {
            ListPremiosTotales = new List<TotalesPorComplejoPremiosModels>();
            ListTotalesPorComplejo = new List<TotalesDiscriminadoPorComplejoModels>();
        }
            
        public string TotalPuntosCargados { get; set; }
        public string TotalPuntosCanjeados { get; set; }
        public string TotalPremiosCanjeados { get; set; }
        public  List<TotalesDiscriminadoPorComplejoModels> ListTotalesPorComplejo { get; set; }
        public List<TotalesPorComplejoPremiosModels> ListPremiosTotales { get; set; }
    }

    public class TotalesDiscriminadoPorComplejoModels
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