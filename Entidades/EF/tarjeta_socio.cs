//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entidades.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class tarjeta_socio
    {
        public int id { get; set; }
        public string numero_tarjeta { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public string usuario_alta { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public string usuario_modificacion { get; set; }
        public Nullable<System.DateTime> fecha_baja { get; set; }
        public bool activada { get; set; }
        public string observaciones { get; set; }
        public string modo_activacion { get; set; }
        public Nullable<int> socio_id { get; set; }
    
        public virtual socio socio { get; set; }
    }
}
