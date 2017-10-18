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
    public class TarjetasController : Controller
    {
        private ComunidadContext db = new ComunidadContext();


        // GET: Tarjetas
        [Authorize(Roles = "TarjetasControllerIndex")]
        public async Task<ActionResult> Index()
        {
            return View(await db.tarjeta_socio.ToListAsync());
        }


        // GET: Tarjetas/Details/5
        [Authorize(Roles = "TarjetasControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tarjeta_socio tarjeta_socio = await db.tarjeta_socio.FindAsync(id);
            if (tarjeta_socio == null)
            {
                return HttpNotFound();
            }
            return View(tarjeta_socio);
        }


        // GET: Tarjetas/Create
        [Authorize(Roles = "TarjetasControllerCreate")]
        public ActionResult Create()
        {
            return View();
        }

        
        // POST: Tarjetas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TarjetasControllerCreate")]
        public async Task<ActionResult> Create([Bind(Include = "numero_tarjeta,activada,observaciones")] tarjeta_socio tarjeta_socio)
        {
            if (ModelState.IsValid)
            {
                tarjeta_socio.fecha_alta = DateTime.Now;
                tarjeta_socio.fecha_modificacion = DateTime.Now;
                tarjeta_socio.modo_activacion = Helper.Helper.ModosActivacionTarjeta.UsuarioInterno.ToString();
                tarjeta_socio.usuario_alta = User.Identity.Name;
                tarjeta_socio.usuario_modificacion = User.Identity.Name;
                db.tarjeta_socio.Add(tarjeta_socio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tarjeta_socio);
        }


        // GET: Tarjetas/Edit/5
        [Authorize(Roles = "TarjetasControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tarjeta_socio tarjeta_socio = await db.tarjeta_socio.FindAsync(id);
            if (tarjeta_socio == null)
            {
                return HttpNotFound();
            }
            return View(tarjeta_socio);
        }

        // POST: Tarjetas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TarjetasControllerEdit")]
        public async Task<ActionResult> Edit([Bind(Include = "id,numero_tarjeta,fecha_alta,usuario_alta,fecha_modificacion,usuario_modificacion,fecha_baja,activada,observaciones,modo_activacion,socio_id")] tarjeta_socio tarjeta_socio)
        {
            if (ModelState.IsValid)
            {
                tarjeta_socio.fecha_alta = DateTime.Now;
                tarjeta_socio.fecha_modificacion = DateTime.Now;
                tarjeta_socio.modo_activacion = Helper.Helper.ModosActivacionTarjeta.UsuarioInterno.ToString();
                tarjeta_socio.usuario_alta = User.Identity.Name;
                tarjeta_socio.usuario_modificacion = User.Identity.Name;
                db.Entry(tarjeta_socio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tarjeta_socio);
        }

        // GET: Tarjetas/Delete/5
        [Authorize(Roles = "TarjetasControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tarjeta_socio tarjeta_socio = await db.tarjeta_socio.FindAsync(id);
            if (tarjeta_socio == null)
            {
                return HttpNotFound();
            }
            return View(tarjeta_socio);
        }

        // POST: Tarjetas/Delete/5
        [Authorize(Roles = "TarjetasControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tarjeta_socio tarjeta_socio = await db.tarjeta_socio.FindAsync(id);
            db.tarjeta_socio.Remove(tarjeta_socio);
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
