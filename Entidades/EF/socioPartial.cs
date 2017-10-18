using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EF
{
    public partial class socio
    {
        /// <summary>
        /// Ojo Solo Se utiliza en el modelo mvc No tocar para otra Cosa
        /// </summary>
        public string NroTarjeta { get; set; }

        public tarjeta_socio TarjetaActual
        {
            get
            {
                if (tarjeta_socio != null && tarjeta_socio.Count > 0)
                {
                  return  tarjeta_socio.FirstOrDefault(t=>t.activada);
                }
                else
                {
                    return new EF.tarjeta_socio();
                }
            }
        }
    }
}
