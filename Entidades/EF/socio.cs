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
    
    public partial class socio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public socio()
        {
            this.canje_premios = new HashSet<canje_premios>();
            this.carga_puntos = new HashSet<carga_puntos>();
            this.tarjeta_socio = new HashSet<tarjeta_socio>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string documento { get; set; }
        public string celular { get; set; }
        public string mail { get; set; }
        public string direccion { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public string usuario_alta { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public string usuario_modificacion { get; set; }
        public Nullable<System.DateTime> fecha_baja { get; set; }
        public string observaciones { get; set; }
        public decimal puntos_actuales { get; set; }
        public Nullable<System.DateTime> fecha_nacimiento { get; set; }
        public Nullable<int> complejo_alta_id { get; set; }
        public string UserName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<canje_premios> canje_premios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<carga_puntos> carga_puntos { get; set; }
        public virtual complejo complejo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tarjeta_socio> tarjeta_socio { get; set; }
    }
}
