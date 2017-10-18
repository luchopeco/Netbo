using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebComunidad.Models
{
    public class ComunidadContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public ComunidadContext() : base("name=ComunidadEntities")
        {
        }



        public System.Data.Entity.DbSet<Entidades.EF.AspNetUser> AspNetUsers { get; set; }

        public System.Data.Entity.DbSet<Entidades.EF.AspNetRole> AspNetRoles { get; set; }

        public System.Data.Entity.DbSet<Entidades.EF.AspNet_Perfiles> AspNet_Perfiles { get; set; }

        public System.Data.Entity.DbSet<Entidades.EF.tipo_beneficio> tipo_beneficio { get; set; }
        public System.Data.Entity.DbSet<Entidades.EF.complejo> complejoes { get; set; }

        public System.Data.Entity.DbSet<Entidades.EF.socio> socios { get; set; }
        public System.Data.Entity.DbSet<Entidades.EF.provincia> provincias { get; set; }
        public System.Data.Entity.DbSet<Entidades.EF.tarjeta_socio> tarjeta_socio { get; set; }
        public System.Data.Entity.DbSet<Entidades.EF.carga_puntos> carga_puntos { get; set; }

        public System.Data.Entity.DbSet<Entidades.EF.premio> premios { get; set; }

        public System.Data.Entity.DbSet<Entidades.EF.canje_premios> canje_premios { get; set; }
    }

}

