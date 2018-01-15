using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComunidad.Models.Socios
{
    public class CambioTarjetaModels
    {
        public Entidades.EF.socio Socio { get; set; }

        public Entidades.EF.tarjeta_socio TarjetaNueva { get; set; }

    }
}