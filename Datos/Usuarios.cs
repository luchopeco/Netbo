using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.EF;

namespace Datos
{
    public class Usuarios
    {
        /// <summary>
        /// Agregar - actualiza los roles de un perfil. El perfil Ya debe esxistri en la DB
        /// </summary>
        public static void AgregarRolesPerfiles(AspNet_Perfiles perfil)
        {
            List<AspNetUser> listUsu;
            using (Entidades.EF.ComunidadEntities context = new Entidades.EF.ComunidadEntities())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM AspNet_PerfilesRoles WHERE PerfilesId = @id", new SqlParameter("id", perfil.id));
                var per = context.AspNet_Perfiles.Find(perfil.id);
                var sql = @"SELECT DISTINCT anu.* FROM AspNet_PerfilesUsuarios anpu
                            INNER JOIN AspNetUsers anu ON anu.Id = anpu.UsuariosId
                            WHERE anpu.PerfilesId = @id";
                listUsu = context.Database.SqlQuery<Entidades.EF.AspNetUser>(sql, new SqlParameter("id", perfil.id)).ToList();
                foreach (var usu in listUsu)
                {
                    usu.AspNet_Perfiles = context.AspNetUsers.Find(usu.Id).AspNet_Perfiles;
                }
                per.AspNetRoles = perfil.AspNetRoles;
                foreach (var r in per.AspNetRoles)
                {
                    context.Entry(r).State = System.Data.Entity.EntityState.Modified;
                }

                context.Entry(per).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            foreach (var usu in listUsu)
            {
                agregarUsarioRoles(usu);
            }

        }
        /// <summary>
        /// Agrega modifica los perfiles del usuario junto a sus roles.
        /// Se encarga de asignar los perfiles y todos sus roles.
        /// </summary>
        /// <param name="usuario"></param>
        public static void AgregarPerfilesUsuario(AspNetUser usuario)
        {
            using (Entidades.EF.ComunidadEntities context = new Entidades.EF.ComunidadEntities())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM AspNet_PerfilesUsuarios WHERE UsuariosId = @id", new SqlParameter("id", usuario.Id));

                var usu = context.AspNetUsers.Find(usuario.Id);

                usu.AspNet_Perfiles = usuario.AspNet_Perfiles;

                foreach (var p in usu.AspNet_Perfiles)
                {

                    context.Entry(p).State = System.Data.Entity.EntityState.Modified;

                }

                context.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            agregarUsarioRoles(usuario);
        }

        private static void agregarUsarioRoles(AspNetUser usuario)
        {
            var sql = @"SELECT DISTINCT anr.Id, anr.Name
                        FROM   AspNet_PerfilesRoles anpr
                               INNER JOIN AspNetRoles anr ON anr.Id = anpr.RolesId
                        WHERE  anpr.PerfilesId IN (";



            int contador = 0;
            int cantidad = usuario.AspNet_Perfiles.Count;
            foreach (var perfil in usuario.AspNet_Perfiles)
            {
                sql = sql + perfil.id;
                contador = contador + 1;
                if (cantidad != contador)
                {
                    sql = sql + ",";
                }
                else
                {
                    sql = sql + ")";
                }
            }
            using (Entidades.EF.ComunidadEntities context = new Entidades.EF.ComunidadEntities())
            {
                var usu = context.AspNetUsers.Find(usuario.Id);

                context.Database.ExecuteSqlCommand("DELETE FROM AspNetUserRoles  WHERE UserId = @id", new SqlParameter("id", usuario.Id));
                var roles = context.Database.SqlQuery<Entidades.EF.AspNetRole>(sql).ToList();
                usu.AspNetRoles = roles;
                foreach (var r in usu.AspNetRoles)
                {
                    context.Entry(r).State = System.Data.Entity.EntityState.Modified;
                }
                context.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Asigna un usuario a un complejo
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idComplejo"></param>
        public static void AsignarComplejoAUsuario(string idUsuario, int idComplejo)
        {
            using (Entidades.EF.ComunidadEntities context = new Entidades.EF.ComunidadEntities())
            {
                var usu = context.AspNetUsers.Find(idUsuario);
                usu.complejo_id = idComplejo;
                context.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static List<string> BuscarRolesGrupoAd(List<string> listGruposAd)
        {
            string grupos = "(";
            foreach (var g in listGruposAd)
            {
                grupos = grupos + "'" + g + "' ,";
            }
            grupos = grupos.Remove(grupos.Length - 1) + ")";
            List<AspNetRole> listUsu;
            using (Entidades.EF.ComunidadEntities context = new Entidades.EF.ComunidadEntities())
            {
                var sql = @"SELECT DISTINCT  anr.* FROM AspNetRoles anr
                            INNER JOIN AspNet_PerfilesRoles anpr ON anpr.RolesId = anr.Id
                            INNER JOIN AspNet_Perfiles anp ON anp.id = anpr.PerfilesId
                            WHERE anp.grupo_ad IN " + grupos;
                listUsu = context.Database.SqlQuery<Entidades.EF.AspNetRole>(sql).ToList();
            }
            List<string> listRoles = new List<string>();
            foreach (var r in listUsu)
            {
                listRoles.Add(r.Name);
            }
            return listRoles;
        }
    }
}
