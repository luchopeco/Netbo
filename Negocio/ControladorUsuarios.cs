using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ControladorUsuarios
    {
        public static void AgregarRolesPerfiles(Entidades.EF.AspNet_Perfiles perfil)
        {
            Datos.Usuarios.AgregarRolesPerfiles(perfil);

        }

        public static void AgregarPerfilesUsuario(Entidades.EF.AspNetUser usuario)
        {
            Datos.Usuarios.AgregarPerfilesUsuario(usuario);

        }

        public static List<string> BuscarRolesParaGrupoAd(List<string> listGruposAd)
        {
            return Datos.Usuarios.BuscarRolesGrupoAd(listGruposAd);
        }

        /// <summary>
        /// Asigna el complejo al usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idComplejo"></param>
        public static void AsignarComplejoAUsuario(string idUsuario, int idComplejo)
        {
            Datos.Usuarios.AsignarComplejoAUsuario(idUsuario, idComplejo);
        }
    }
}
