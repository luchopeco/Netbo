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
    public class TiposBeneficiosController : Controller
    {
        private ComunidadContext db = new ComunidadContext();

        // GET: tipo_beneficio
        [Authorize(Roles = "TiposBeneficiosControllerIndex")]
        public async Task<ActionResult> Index()
        {
            return View(await db.tipo_beneficio.ToListAsync());
        }

        // GET: tipo_beneficio/Details/5
        [Authorize(Roles = "TiposBeneficiosControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_beneficio tipo_beneficio = await db.tipo_beneficio.FindAsync(id);
            if (tipo_beneficio == null)
            {
                return HttpNotFound();
            }
            return View(tipo_beneficio);
        }

        // GET: tipo_beneficio/Create
        [Authorize(Roles = "TiposBeneficiosControllerCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipo_beneficio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "TiposBeneficiosControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,descripcion,monto_dinero,puntos")] tipo_beneficio tipo_beneficio)
        {
            if (ModelState.IsValid)
            {
                tipo_beneficio.fecha_alta = DateTime.Now;
                tipo_beneficio.fecha_modificacion = DateTime.Now;
                tipo_beneficio.usuario_alta = User.Identity.Name;
                tipo_beneficio.usuario_modificacion = User.Identity.Name;
                db.tipo_beneficio.Add(tipo_beneficio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tipo_beneficio);
        }

        // GET: tipo_beneficio/Edit/5
        [Authorize(Roles = "TiposBeneficiosControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_beneficio tipo_beneficio = await db.tipo_beneficio.FindAsync(id);
            if (tipo_beneficio == null)
            {
                return HttpNotFound();
            }
            return View(tipo_beneficio);
        }

        // POST: tipo_beneficio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "TiposBeneficiosControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,descripcion,monto_dinero,puntos,fecha_alta,usuario_alta")] tipo_beneficio tipo_beneficio)
        {
            if (ModelState.IsValid)
            {
                tipo_beneficio.fecha_modificacion = DateTime.Now;                
                tipo_beneficio.usuario_modificacion = User.Identity.Name;

                db.Entry(tipo_beneficio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tipo_beneficio);
        }

        // GET: tipo_beneficio/Delete/5
        [Authorize(Roles = "TiposBeneficiosControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_beneficio tipo_beneficio = await db.tipo_beneficio.FindAsync(id);
            if (tipo_beneficio == null)
            {
                return HttpNotFound();
            }
            return View(tipo_beneficio);
        }

        // POST: tipo_beneficio/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "TiposBeneficiosControllerDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tipo_beneficio tipo_beneficio = await db.tipo_beneficio.FindAsync(id);
            db.tipo_beneficio.Remove(tipo_beneficio);
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
