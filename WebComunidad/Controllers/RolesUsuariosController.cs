using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Entidades.EF;
using WebComunidad.Models;

namespace WebComunidad.Controllers
{
    [Authorize]
    public class RolesUsuariosController : Controller
    {
        private ComunidadContext db = new ComunidadContext();

        [Authorize(Roles = "RolesUsuariosControllerEdit")]
        // GET: RolesUsuarios
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser usuario = await db.AspNetUsers.FindAsync(id);
            List<AspNetRole> roles = await db.AspNetRoles.ToListAsync();

            //Assembly asm = Assembly.GetAssembly(typeof(WebComunidad.MvcApplication));
            //var controlleractionlist = asm.GetTypes()
            //        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
            //        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            //        .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
            //        .Where(x => x.Attributes.Contains("Authorize"))
            //        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            ///List<AspNetRole> listRolesDistinct = new List<AspNetRole>();
            foreach (var cont in roles)
            {               
                if (usuario.AspNetRoles.ToList().Exists(rr => rr.Name == cont.Name))
                {
                    cont.Activo = true;                                      
                }
            }

            ViewBag.IdUsuario = usuario.Id;
            ViewBag.NombreUsuario = usuario.UserName;
            return View(roles);

        }

        [Authorize(Roles = "RolesUsuariosControllerEdit")]
        [HttpPost]
        public async Task<ActionResult> Edit(List<AspNetRole> listRoles)
        {
            var caca = ViewBag.IdUsuario;

            return View(listRoles);
        }
    }
}