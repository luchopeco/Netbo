using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSociosComunidad.Models.Canjes
{
    public class index
    {
        public index()
        {
            ListcanjesBusqueda = new List<CanjesPremios>();
            ListCanjesUltimoMes = new List<CanjesPremios>();
        }
        public int TotalCanjes { get; set; }

        public List<CanjesPremios> ListCanjesUltimoMes { get; set; }

        public List<CanjesPremios> ListcanjesBusqueda { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

    }
    public class CanjesPremios
    {
        public int PuntosCanjeados { get; set; }
        public string Premio { get; set; }

        public string ComplejoDesc { get; set; }

        public DateTime FechaCanje { get; set; }
    }
}