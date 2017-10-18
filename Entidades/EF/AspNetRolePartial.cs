using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EF
{
    public partial class AspNetRole
    {
        /// <summary>
        /// Indica si un rol esta activo en un usuario
        /// </summary>
        public bool Activo { get; set; }
    }
}
