using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSociosComunidad.Models
{
    public class HomeModels
    {
        public int PuntosDisponibles { get; set; }

        public string UltimaCargaDePuntos { get; set; }

        public int CantidadPremiosCanjeados { get; set; }

        public string UltimoPremioCanjeado { get; set; }
    }
}