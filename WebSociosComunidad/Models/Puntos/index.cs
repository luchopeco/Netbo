using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSociosComunidad.Models.Puntos
{
    public class index
    {
        public index()
        {
            ListCargasBusqueda = new List<CargasPuntos>();
            ListCargasUltimoMes = new List<CargasPuntos>();
        }
        public int TotalPuntos { get; set; }

        public List<CargasPuntos> ListCargasUltimoMes { get; set; }

        public List<CargasPuntos> ListCargasBusqueda { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

    }
    public class CargasPuntos
    {
        public int Puntos { get; set; }
        public string TipoBeneficioDesc { get; set; }

        public string ComplejoDesc { get; set; }

        public  DateTime FechaCarga { get; set; }
    }
}