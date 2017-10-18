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
    
    public partial class canje_premios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public canje_premios()
        {
            this.canje_premios_detalle = new HashSet<canje_premios_detalle>();
        }
    
        public int id { get; set; }
        public int socio_id { get; set; }
        public int premio_id { get; set; }
        public decimal puntos_canjeados { get; set; }
        public int complejo_canje_id { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public string observaciones { get; set; }
        public decimal premio_puntos_valor { get; set; }
        public Nullable<int> premio_complejo_responsable_id { get; set; }
        public string usuario_alta { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public string usaurio_modificacion { get; set; }
    
        public virtual complejo complejo { get; set; }
        public virtual complejo complejo1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<canje_premios_detalle> canje_premios_detalle { get; set; }
        public virtual premio premio { get; set; }
        public virtual socio socio { get; set; }
    }
}