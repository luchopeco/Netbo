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
    
    public partial class canje_premios_detalle
    {
        public int canje_premio_id { get; set; }
        public int carga_puntos_id { get; set; }
        public decimal puntos_utilizados { get; set; }
    
        public virtual carga_puntos carga_puntos { get; set; }
        public virtual canje_premios canje_premios { get; set; }
    }
}