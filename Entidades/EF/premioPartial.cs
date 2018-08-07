using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entidades.EF
{
    public partial class premio
    {
        public HttpPostedFileBase ImagenFile { get; set; }

        /// <summary>
        /// Url para mostrar la imagen
        /// </summary>
        public string UrlImagen
        {
            get
            {
                if (!String.IsNullOrEmpty(path_imagenes))
                {
                    return "/Content/img/premios/"+path_imagenes;
                }
                else
                {
                    return "/Content/img/premios/premioDefecto.png";
                }
                

            }
        }
    }
}
