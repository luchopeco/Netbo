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
    public class PremiosController : Controller
    {
        private ComunidadContext db = new ComunidadContext();

        // GET: premios
        [Authorize(Roles = "PremiosControllerIndex")]
        public async Task<ActionResult> Index()
        {
            return View(await db.premios.ToListAsync());
        }

        // GET: premios/Details/5
        [Authorize(Roles = "PremiosControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            premio premio = await db.premios.FindAsync(id);
            if (premio == null)
            {
                return HttpNotFound();
            }
            return View(premio);
        }

        // GET: premios/Create
        [Authorize(Roles = "PremiosControllerCreate")]
        public ActionResult Create()
        {            
            ViewBag.ListComplejos = new SelectList(
                 db.complejoes.Where(cc => cc.fecha_baja == null),
                 "id", "descripcion");
            return View();
        }

        // POST: premios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "PremiosControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "nombre,descripcion_larga,puntos,complejo_responsable_id,desactivado")] premio premio)
        {
            if (ModelState.IsValid)
            {
                premio.usuario_alta = User.Identity.Name;
                premio.usuario_modificacion = User.Identity.Name;
                premio.fecha_alta = DateTime.Now;
                premio.fecha_modificacion = DateTime.Now;
                db.premios.Add(premio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(premio);
        }

        // GET: premios/Edit/5
        [Authorize(Roles = "PremiosControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ListComplejos = new SelectList(
               db.complejoes.Where(cc => cc.fecha_baja == null),
               "id", "descripcion");
            premio premio = await db.premios.FindAsync(id);
            if (premio == null)
            {
                return HttpNotFound();
            }
            return View(premio);
        }

        // POST: premios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "PremiosControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombre,descripcion_larga,puntos,complejo_responsable_id,desactivado,usuario_alta,fecha_alta")] premio premio)
        {
            if (ModelState.IsValid)
            {            
                premio.usuario_modificacion = User.Identity.Name;              
                premio.fecha_modificacion = DateTime.Now;
                db.Entry(premio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(premio);
        }

        // GET: premios/Delete/5
        [Authorize(Roles = "PremiosControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            premio premio = await db.premios.FindAsync(id);
            if (premio == null)
            {
                return HttpNotFound();
            }
            return View(premio);
        }

        // POST: premios/Delete/5
        [Authorize(Roles = "PremiosControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            premio premio = await db.premios.FindAsync(id);
            db.premios.Remove(premio);
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
