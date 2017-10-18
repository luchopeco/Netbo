using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.CargaPuntos
{
    public class CargaPuntosIndexModels
    {
        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public List<Entidades.EF.carga_puntos> ListCargaPuntos { get; set; }
    }
}