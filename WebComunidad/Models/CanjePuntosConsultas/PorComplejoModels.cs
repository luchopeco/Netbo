using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.CanjePuntosConsultas
{
    public class PorComplejoModels
    {
        public int IdComplejo { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public List<Entidades.EF.canje_premios> ListCanjes { get; set; }
    }
}