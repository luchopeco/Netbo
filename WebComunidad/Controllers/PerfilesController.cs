using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entidades.EF;
using WebComunidad.Models;


namespace WebComunidad.Controllers
{
    public class PerfilesController : Controller
    {


        private ComunidadContext db = new ComunidadContext();

        // GET: Perfiles
        [Authorize(Roles = "PerfilesControllerIndex")]
        public async Task<ActionResult> Index()
        {
            return View(await db.AspNet_Perfiles.ToListAsync());
        }

        // GET: Perfiles/Details/5
        [Authorize(Roles = "PerfilesControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNet_Perfiles perfil = await db.AspNet_Perfiles.FindAsync(id);
            List<AspNetRole> roles = await db.AspNetRoles.ToListAsync();
            foreach (var cont in roles)
            {
                if (perfil.AspNetRoles.ToList().Exists(rr => rr.Name == cont.Name))
                {
                    cont.Activo = true;
                }
            }
            if (perfil == null)
            {
                return HttpNotFound();
            }

            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }

            ViewBag.IdPerfil = perfil.id;
            ViewBag.Perfil = perfil.descripcion;            
            return View(roles);
        }

        // GET: Perfiles/Create
        [Authorize(Roles = "PerfilesControllerCreate")]
        public ActionResult Create()
        {         
            return View();
        }

        // POST: Perfiles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "PerfilesControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,descripcion")] AspNet_Perfiles aspNet_Perfiles)
        {
            if (ModelState.IsValid)
            {
                db.AspNet_Perfiles.Add(aspNet_Perfiles);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aspNet_Perfiles);
        }

        // GET: Perfiles/Edit/5
        [Authorize(Roles = "PerfilesControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            AspNet_Perfiles aspNet_Perfiles = await db.AspNet_Perfiles.FindAsync(id);
            if (aspNet_Perfiles == null)
            {
                return HttpNotFound();
            }
            return View(aspNet_Perfiles);
        }

        // POST: Perfiles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "PerfilesControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,descripcion")] AspNet_Perfiles aspNet_Perfiles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNet_Perfiles).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNet_Perfiles);
        }

        // GET: Perfiles/Delete/5
        [Authorize(Roles = "PerfilesControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNet_Perfiles aspNet_Perfiles = await db.AspNet_Perfiles.FindAsync(id);
            if (aspNet_Perfiles == null)
            {
                return HttpNotFound();
            }
            return View(aspNet_Perfiles);
        }

        // POST: Perfiles/Delete/5
        [Authorize(Roles = "PerfilesControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AspNet_Perfiles aspNet_Perfiles = await db.AspNet_Perfiles.FindAsync(id);
            db.AspNet_Perfiles.Remove(aspNet_Perfiles);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(Roles = "PerfilesControllerRolesPerfil")]
        public async Task<ActionResult> RolesPerfil(List<AspNetRole> listRoles)
        {
            var idPerfil = Request.Form["id-perfil"];
            Entidades.EF.AspNet_Perfiles perf = new AspNet_Perfiles();
            perf.id = Convert.ToInt32(idPerfil);
            foreach (var r in listRoles)
            {
                if (r.Activo)
                {
                    perf.AspNetRoles.Add(r);
                }
            }
            Negocio.ControladorUsuarios.AgregarRolesPerfiles(perf);


            //var perf = db.AspNet_Perfiles.Find(Convert.ToInt32(idPerfil));
            //db.Entry(perf).State = EntityState.Modified;
            //perf.AspNetRoles.Clear();


            //foreach (var r in listRoles)
            //{
            //    if (perf.AspNetRoles.Any(rr=>rr.Id == r.Id))
            //    {
            //        if (r.Activo)
            //        {
            //            db.Entry(r).State = EntityState.Unchanged;
            //            perf.AspNetRoles.Add(r);
            //        }
            //        else
            //        {
            //            db.Entry(r).State = EntityState.Unchanged;
            //            perf.AspNetRoles.Add(r);
            //        }
            //    }


            //}
            //await db.SaveChangesAsync();
            TempData["MsjExito"] = "Roles Modificados Correctamente";
            return RedirectToAction("Details", "Perfiles", new { id = idPerfil });
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
