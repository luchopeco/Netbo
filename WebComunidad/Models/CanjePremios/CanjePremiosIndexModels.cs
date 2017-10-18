using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.CanjePremios
{
    public class CanjePremiosIndexModels
    {
        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public List<Entidades.EF.canje_premios> ListCanjes { get; set; }
    }
}