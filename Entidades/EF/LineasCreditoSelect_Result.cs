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
    
    public partial class LineasCreditoSelect_Result
    {
        public int id { get; set; }
        public int linea_eiv_id { get; set; }
        public int tipo_demostracion_id { get; set; }
        public Nullable<int> codigo_linea { get; set; }
        public string descripcion { get; set; }
        public double tasa_liq { get; set; }
        public double importe_maximo { get; set; }
        public Nullable<int> plazo { get; set; }
        public Nullable<int> agrupacion { get; set; }
        public Nullable<System.DateTime> fecha_hasta { get; set; }
        public int tipo_vehiculo_id { get; set; }
        public double aforo { get; set; }
        public double rci { get; set; }
        public int tipo_linea_id { get; set; }
        public Nullable<int> anio_antiguedad { get; set; }
        public Nullable<int> moneda { get; set; }
        public Nullable<double> ingreso_minimo { get; set; }
        public Nullable<int> antiguedad_laoral_minima { get; set; }
        public Nullable<double> endeudamiento_maximo { get; set; }
        public Nullable<bool> es_0_km { get; set; }
        public Nullable<int> modelo { get; set; }
    }
}
