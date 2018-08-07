using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.Socios
{
    public class CanjePremiosModels
    {
        public CanjePremiosModels()
        {
            ListCanjes = new List<CanjesPremios>();            
        }
        public int TotalCanjes { get; set; }
        public int IdSocio { get; set; }
        public string ApenomSocio { get; set; }
        public List<CanjesPremios> ListCanjes { get; set; }
        
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

    }
    public class CanjesPremios
    {
        public int PuntosCanjeados { get; set; }
        public string Premio { get; set; }

        public string PathImagenPremio { get; set; }

        public string ComplejoDesc { get; set; }

        public DateTime FechaCanje { get; set; }
    }
}
