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

    public class ComplejosController : Controller
    {
        private ComunidadContext db = new ComunidadContext();


        // GET: Complejos
        [Authorize(Roles = "ComplejosControllerIndex")]
        public async Task<ActionResult> Index()
        {
            return View(await db.complejoes.ToListAsync());
        }


        // GET: Complejos/Details/5
        [Authorize(Roles = "ComplejosControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complejo complejo = await db.complejoes.FindAsync(id);
            if (complejo == null)
            {
                return HttpNotFound();
            }
            return View(complejo);
        }


        // GET: Complejos/Create
        [Authorize(Roles = "ComplejosControllerCreate")]
        public ActionResult Create()
        {
            return View();
        }


        // POST: Complejos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ComplejosControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,descripcion,fecha_alta,fecha_baja,usuario_alta,fecha_modificacion,usuario_modificacion,usuario_baja")] complejo complejo)
        {
            if (ModelState.IsValid)
            {
                complejo.fecha_alta = DateTime.Now;
                complejo.fecha_modificacion = DateTime.Now;
                complejo.usuario_alta = User.Identity.Name;
                complejo.usuario_modificacion = User.Identity.Name;
                db.complejoes.Add(complejo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(complejo);
        }


        // GET: Complejos/Edit/5
        [Authorize(Roles = "ComplejosControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complejo complejo = await db.complejoes.FindAsync(id);
            if (complejo == null)
            {
                return HttpNotFound();
            }
            return View(complejo);
        }


        // POST: Complejos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener         
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ComplejosControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,descripcion,fecha_alta,fecha_baja,usuario_alta,fecha_modificacion,usuario_modificacion,usuario_baja")] complejo complejo)
        {
            if (ModelState.IsValid)
            {
                complejo.fecha_modificacion = DateTime.Now;
                complejo.usuario_modificacion = User.Identity.Name;
                db.Entry(complejo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(complejo);
        }


        // GET: Complejos/Delete/5
        [Authorize(Roles = "ComplejosControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complejo complejo = await db.complejoes.FindAsync(id);
            if (complejo == null)
            {
                return HttpNotFound();
            }
            return View(complejo);
        }


        // POST: Complejos/Delete/5
        [Authorize(Roles = "ComplejosControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            complejo complejo = await db.complejoes.FindAsync(id);
            db.complejoes.Remove(complejo);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
