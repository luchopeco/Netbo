using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Entidades.EF;
using WebComunidad.Models;

namespace WebComunidad.Controllers
{
    public class RolesController : Controller
    {
        private ComunidadContext db = new ComunidadContext();

        // GET: Roles
        [Authorize(Roles = "RolesControllerIndex")]
        public async Task<ActionResult> Index()
        {
            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }

            return View(await db.AspNetRoles.ToListAsync());
        }

        // GET: Roles/Details/5
        [Authorize(Roles = "RolesControllerDetails")]        
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // GET: Roles/Create
        [Authorize(Roles = "RolesControllerCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RolesControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32( db.AspNetRoles.OrderByDescending(r => r.Id).First().Id);
                aspNetRole.Id = (id + 1).ToString();
                db.AspNetRoles.Add(aspNetRole);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aspNetRole);
        }

        // GET: Roles/Edit/5
        [Authorize(Roles = "RolesControllerEdit")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: Roles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "RolesControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetRole).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetRole);
        }

        // GET: Roles/Delete/5
        [Authorize(Roles = "RolesControllerDelete")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: Roles/Delete/5
        [Authorize(Roles = "RolesControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            db.AspNetRoles.Remove(aspNetRole);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "RolesControllerActualizar")]
        public async Task<ActionResult> Actualizar()
        {
            Assembly asm = Assembly.GetAssembly(typeof(WebComunidad.MvcApplication));
            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(
                    type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(
                    m =>
                        !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true)
                            .Any())
                .Select(
                    x =>
                        new
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes =
                                String.Join(",",
                                    x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                        })
                .Where(x => x.Attributes.Contains("Authorize"))
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList()
                .Select(z=> new {
                    Controller = z.Controller,
                    Action = z.Action,
                }).Distinct();

            List<AspNetRole> listRolesAAgregar = new List<AspNetRole>();
            var rolesDb = db.AspNetRoles.ToList();
            int maxId = Convert.ToInt32(rolesDb.Max(ro => Convert.ToInt32(ro.Id)));
            foreach (var r in controlleractionlist)
            {
                maxId = maxId + 1;
                AspNetRole rol = new AspNetRole();
                rol.Name = r.Controller + r.Action;
                rol.Id = maxId.ToString();
                if (!rolesDb.Exists(rrr => rrr.Name == rol.Name))
                {
                    db.Entry(rol).State = EntityState.Added;
                    db.AspNetRoles.Add(rol);
                }
            }
            db.SaveChanges();

            TempData["MsjExito"] = "Roles Actualizados Con Exito";
            return RedirectToAction("Index", "Roles");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
