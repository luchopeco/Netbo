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
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace WebComunidad.Controllers
{
    public class SociosController : Controller
    {
        public int IdComplejoIdentity
        {
            get
            {
                return Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirstValue("IdComplejo"));
            }
        }
        private ComunidadContext db = new ComunidadContext();

        // GET: Socios
        [Authorize(Roles = "SociosControllerIndex")]
        public async Task<ActionResult> Index()
        {
            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }
            return View(await db.socios.ToListAsync());
        }

        // GET: Socios/Details/5
        [Authorize(Roles = "SociosControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            socio socio = await db.socios.FindAsync(id);
            if (socio == null)
            {
                return HttpNotFound();
            }
            return View(socio);
        }

        // GET: Socios/Create
        [Authorize(Roles = "SociosControllerCreate")]
        public ActionResult Create()
        {
            if (TempData["MsjError"] != null)
            {
                ViewBag.MsjError = TempData["MsjError"];
                TempData.Remove("MsjError");
            }
            cargarVieBagProvincias();
            return View();
        }

        // POST: Socios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SociosControllerCreate")]
        public async Task<ActionResult> Create([Bind(Include = "id,nombre,apellido,documento,celular,mail,direccion,localidad,provincia,observaciones,NroTarjeta,fecha_nacimiento")] socio socio)
        {
            if (ModelState.IsValid)
            {
                if (db.socios.Any(ss => ss.documento == socio.documento))
                {
                    TempData["MsjError"] = "Ya existe un socio con el documento indicado";
                    return RedirectToAction("Create");
                }
                try
                {
                    socio.complejo_alta_id = IdComplejoIdentity;

                    var t = db.tarjeta_socio.FirstOrDefault(ts => ts.numero_tarjeta == socio.NroTarjeta.Trim());
                    if (t != null)
                    {
                        if (t.socio_id != null)
                        {
                            TempData["MsjError"] = "La tarjeta esta asociada a otro socio";
                            return RedirectToAction("Create");
                        }
                        else if (!t.activada)
                        {
                            TempData["MsjError"] = "La tarjeta se encuentra inactiva";
                            return RedirectToAction("Create");
                        }
                        else
                        {
                            using (var dbTransaction = db.Database.BeginTransaction())
                            {
                                try
                                {
                                    socio.fecha_alta = DateTime.Now;
                                    socio.fecha_modificacion = DateTime.Now;
                                    socio.usuario_alta = User.Identity.Name;
                                    socio.usuario_modificacion = User.Identity.Name;
                                    db.socios.Add(socio);
                                    await db.SaveChangesAsync();

                                    t.activada = true;
                                    t.fecha_modificacion = DateTime.Now;
                                    t.modo_activacion = Helper.Helper.ModosActivacionTarjeta.UsuarioInterno.ToString();
                                    t.usuario_modificacion = User.Identity.Name;
                                    t.socio_id = socio.id;
                                    db.Entry(t).State = EntityState.Modified;
                                    db.Configuration.ValidateOnSaveEnabled = false;
                                    await db.SaveChangesAsync();

                                    dbTransaction.Commit();

                                }
                                catch (Exception)
                                {
                                    dbTransaction.Rollback();
                                    throw;
                                }

                            }
                        }
                    }
                    else
                    {
                        using (var dbTransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                socio.fecha_alta = DateTime.Now;
                                socio.fecha_modificacion = DateTime.Now;
                                socio.usuario_alta = User.Identity.Name;
                                socio.usuario_modificacion = User.Identity.Name;
                                if (!String.IsNullOrEmpty(socio.NroTarjeta))
                                {
                                    t = new tarjeta_socio();
                                    t.activada = true;
                                    t.fecha_alta = DateTime.Now;
                                    t.fecha_modificacion = DateTime.Now;
                                    t.modo_activacion = Helper.Helper.ModosActivacionTarjeta.UsuarioInterno.ToString();
                                    t.numero_tarjeta = socio.NroTarjeta;
                                    t.usuario_alta = User.Identity.Name;
                                    t.usuario_modificacion = User.Identity.Name;
                                    t.socio = socio;
                                    db.tarjeta_socio.Add(t);
                                    await db.SaveChangesAsync();
                                }
                                else
                                {
                                    db.socios.Add(socio);
                                    await db.SaveChangesAsync();
                                }
                                dbTransaction.Commit();

                            }
                            catch (Exception ex)
                            {

                                dbTransaction.Rollback();
                                throw;
                            }
                        }


                    }
                    TempData["MsjExito"] = "Socio Agregado Con Exito";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["MsjError"] = ex.Message;
                    return RedirectToAction("Create");

                }
            }

            return View(socio);
        }

        // GET: Socios/Edit/5
        [Authorize(Roles = "SociosControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (TempData["MsjError"] != null)
            {
                ViewBag.MsjError = TempData["MsjError"];
                TempData.Remove("MsjError");
            }
            cargarVieBagProvincias();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            socio socio = await db.socios.FindAsync(id);
            if (socio == null)
            {
                return HttpNotFound();
            }
            return View(socio);
        }

        // POST: Socios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SociosControllerEdit")]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombre,apellido,documento,celular,mail,direccion,localidad,provincia,fecha_alta,usuario_alta,observaciones,fecha_nacimiento,complejo_alta_id")] socio socio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    socio.fecha_modificacion = DateTime.Now;
                    socio.usuario_modificacion = User.Identity.Name;
                    db.Entry(socio).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["MsjExito"] = "Socio Modificado Con Exito";
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("UNIQUE KEY"))
                    {
                        ViewBag.MsjError = "Ya existe un socio con el documento indicado";
                        cargarVieBagProvincias();
                        return View(socio);
                    }
                }

            }
            return View(socio);
        }

        // GET: Socios/Delete/5
        [Authorize(Roles = "SociosControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            socio socio = await db.socios.FindAsync(id);
            if (socio == null)
            {
                return HttpNotFound();
            }
            return View(socio);
        }

        // POST: Socios/Delete/5
        [Authorize(Roles = "SociosControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            socio socio = await db.socios.FindAsync(id);
            db.socios.Remove(socio);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SociosControllerPuntosCargados")]
        [HttpGet]
        public async Task<ActionResult> PuntosCargados(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                if (TempData["MsjExito"] != null)
                {
                    ViewBag.MsjExito = TempData["MsjExito"];
                    TempData.Remove("MsjExito");
                }
                DateTime fd = Helper.Helper.FechaHoraDesde(DateTime.Today);
                DateTime fh = Helper.Helper.FechaHoraHasta(DateTime.Today);
                socio soc = db.socios.Find(id);
                ViewBag.Puntos = (int)soc.puntos_actuales;
                ViewBag.SubTitulo = "Socio: " + soc.apellido + ", " + soc.nombre + " del " + fd.ToShortDateString() + " al " + fh.ToShortDateString();
                var puntos = await (from p in db.carga_puntos
                                    where p.socio_id == id && p.fecha_alta >= fd && p.fecha_alta <= fh
                                    select p).ToListAsync();
                return View(puntos);

            }

        }

        [Authorize(Roles = "SociosControllerPuntosCargados")]
        [HttpPost]
        public async Task<ActionResult> PuntosCargados(int? id, DateTime fechaDesde, DateTime fechaHasta)
        {

            DateTime fd = Helper.Helper.FechaHoraDesde(fechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(fechaHasta);
            socio soc = db.socios.Find(id);
            ViewBag.Puntos = (int)soc.puntos_actuales;
            ViewBag.SubTitulo = "Socio: " + soc.apellido + ", " + soc.nombre + " del " + fd.ToShortDateString() + " al " + fh.ToShortDateString();
            var puntos = await (from p in db.carga_puntos
                                where p.socio_id == id && p.fecha_alta >= fd && p.fecha_alta <= fh
                                select p).ToListAsync();
            return View(puntos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private void cargarVieBagProvincias()
        {
            ViewBag.ListProvincias = new SelectList(
               db.provincias,
               "descripcion", "descripcion");
        }
    }
}
