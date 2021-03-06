﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebSociosComunidad.Models
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

        public System.Data.Entity.DbSet<Entidades.EF.socio> socios { get; set; }
        public System.Data.Entity.DbSet<Entidades.EF.canje_premios> canje_premios { get; set; }
        public System.Data.Entity.DbSet<Entidades.EF.premio> premios { get; set; }
        public System.Data.Entity.DbSet<Entidades.EF.carga_puntos> carga_puntos { get; set; }

    }
}