using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.SociosConsulta
{
    public class RankingCargasModels
    {

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public int CantidadSociosAMostrar { get; set; }

        public int IdSocio { get; set; }

        public string ApenomSocio { get; set; }

        public string Documento { get; set; }

        public string Celular { get; set; }

        public string Obs { get; set; }

        public string ComplejoAlta { get; set; }

        public int CantidadCargas { get; set; }

        public DateTime FechaUltimaCarga { get; set; }
        public string FechaUltimaCargaAmigable {
            get
            {
                if (FechaUltimaCarga != null)
                {
                    return Helper.Helper.FechaAmigable(FechaUltimaCarga);
                }
                else
                {
                    return "";
                }
            }
        }
        public string ComplejoUltimaCarga { get; set; }

        public int PuntosActuales { get; set; }
    }
}