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
    public class CanjePremiosController : Controller
    {
        public int IdComplejoIdentity
        {
            get
            {
                return Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirstValue("IdComplejo"));
            }
        }
        public string ComplejoDescIdentity
        {
            get
            {
                return ((ClaimsIdentity)User.Identity).FindFirstValue("Complejo");
            }
        }
        private ComunidadContext db = new ComunidadContext();

        // GET: CanjePremios
        [Authorize(Roles = "CanjePremiosControllerIndex")]
        public async Task<ActionResult> Index()
        {
            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }
            Models.CanjePremios.CanjePremiosIndexModels cp = new Models.CanjePremios.CanjePremiosIndexModels();
            cp.FechaDesde = DateTime.Today;
            cp.FechaHasta = DateTime.Today;
            DateTime fd = Helper.Helper.FechaHoraDesde(DateTime.Today);
            DateTime fh = Helper.Helper.FechaHoraHasta(DateTime.Today);
            ViewBag.Complejo = ComplejoDescIdentity;
            cp.ListCanjes = await db.canje_premios.Where(c => c.complejo_canje_id == IdComplejoIdentity && c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View(cp);
        }
        [Authorize(Roles = "CanjePremiosControllerTodos")]
        public async Task<ActionResult> Todos()
        {
            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }
            Models.CanjePremios.CanjePremiosIndexModels cp = new Models.CanjePremios.CanjePremiosIndexModels();
            cp.FechaDesde = DateTime.Today;
            cp.FechaHasta = DateTime.Today;
            DateTime fd = Helper.Helper.FechaHoraDesde(DateTime.Today);
            DateTime fh = Helper.Helper.FechaHoraHasta(DateTime.Today);
            ViewBag.Complejo = "Todos";
            cp.ListCanjes = await db.canje_premios.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View(cp);
        }

        [Authorize(Roles = "CanjePremiosControllerPorFecha")]
        [HttpPost]
        public async Task<ActionResult> PorFecha(Models.CanjePremios.CanjePremiosIndexModels cp)
        {

            DateTime fd = Helper.Helper.FechaHoraDesde(cp.FechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(cp.FechaHasta);
            ViewBag.Complejo = ComplejoDescIdentity;
            cp.ListCanjes = await db.canje_premios.Where(c => c.complejo_canje_id == IdComplejoIdentity && c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View("index", cp);

        }
        [Authorize(Roles = "CanjePremiosControllerPorFechaTodos")]
        [HttpPost]
        public async Task<ActionResult> PorFechaTodos(Models.CanjePremios.CanjePremiosIndexModels cp)
        {

            ViewBag.Complejo = "Todos";
            DateTime fd = Helper.Helper.FechaHoraDesde(cp.FechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(cp.FechaHasta);
            ViewBag.Complejo = "Todos";
            cp.ListCanjes = await db.canje_premios.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View("Todos", cp);


        }
        // GET: CanjePremios/Details/5
        [Authorize(Roles = "CanjePremiosControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canje_premios canje_premios = await db.canje_premios.FindAsync(id);
            if (canje_premios == null)
            {
                return HttpNotFound();
            }
            return View(canje_premios);
        }

        // GET: CanjePremios/Create
        [Authorize(Roles = "CanjePremiosControllerCreate")]
        public ActionResult Create()
        {
            var listPremios = from p in db.premios
                              where p.desactivado == false
                              select new
                              {
                                  id = p.id,
                                  nombre = p.nombre + " | " + p.puntos + " puntos"
                              };
            ViewBag.ListPremios = new SelectList(listPremios, "id", "nombre");
            return View();
        }

        // POST: CanjePremios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "CanjePremiosControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "socio_id,premio_id,puntos_canjeados,observaciones")] canje_premios canje_premios)
        {
            if (ModelState.IsValid)
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var premio = db.premios.Find(canje_premios.premio_id);

                        ///completo el canje
                        canje_premios.complejo_canje_id = IdComplejoIdentity;
                        canje_premios.fecha_alta = DateTime.Now;
                        canje_premios.fecha_modificacion = DateTime.Now;
                        canje_premios.premio_complejo_responsable_id = premio.complejo_responsable_id;
                        canje_premios.premio_puntos_valor = premio.puntos;
                        canje_premios.usaurio_modificacion = User.Identity.Name;
                        canje_premios.usuario_alta = User.Identity.Name;

                        ///busco las cargas de puntos del socio ordenadas por fecha y que tengan puntos disponibles
                        var listCargas = from c in db.carga_puntos
                                         where c.socio_id == canje_premios.socio_id
                                         && c.puntos_disponibles > 0
                                         orderby c.fecha_alta ascending
                                         select c;

                        var puntosADescontar = canje_premios.puntos_canjeados;
                        List<carga_puntos> listCargasAModificar = new List<carga_puntos>();
                        ///recorro las cargas con puntos disponibles para descontrales los puntos
                        ///y crear el detalle del canje
                        foreach (var c in listCargas)
                        {
                            if (c.puntos_disponibles >= puntosADescontar)
                            {
                                c.puntos_disponibles = c.puntos_disponibles - puntosADescontar;

                                canje_premios_detalle cd = new canje_premios_detalle();
                                cd.carga_puntos_id = c.id;
                                cd.puntos_utilizados = puntosADescontar;
                                canje_premios.canje_premios_detalle.Add(cd);

                                listCargasAModificar.Add(c);
                                break;
                            }
                            else
                            {
                                puntosADescontar = puntosADescontar - c.puntos_disponibles;

                                canje_premios_detalle cd = new canje_premios_detalle();
                                cd.carga_puntos_id = c.id;
                                cd.puntos_utilizados = c.puntos_disponibles;
                                canje_premios.canje_premios_detalle.Add(cd);

                                c.puntos_disponibles = 0;
                                listCargasAModificar.Add(c);
                            }
                        }


                        ///Le descuentos los puntos actuales al socio
                        var so = db.socios.Find(canje_premios.socio_id);
                        so.puntos_actuales = so.puntos_actuales - canje_premios.puntos_canjeados;
                        db.Entry(so).State = EntityState.Modified;

                        //Agrego al contexto las cargas a modificar
                        foreach (var cc in listCargasAModificar)
                        {
                            db.Entry(cc).State = EntityState.Modified;
                        }
                        ///agrego el canje
                        db.canje_premios.Add(canje_premios);
                        await db.SaveChangesAsync();
                        dbTransaction.Commit();
                        TempData["MsjExito"] = "Canje de Premio Realizado Con Exito";
                        return RedirectToAction("Index");

                    }
                    catch (Exception)
                    {

                        dbTransaction.Rollback();
                        var listPremio = from p in db.premios
                                         where p.desactivado == false
                                         select new
                                         {
                                             id = p.id,
                                             nombre = p.nombre + " | " + p.puntos + " puntos"
                                         };
                        ViewBag.ListPremios = new SelectList(listPremio, "id", "nombre");
                        return View(canje_premios);
                    }
                }

            }

            var listPremios = from p in db.premios
                              where p.desactivado == false
                              select new
                              {
                                  id = p.id,
                                  nombre = p.nombre + " | " + p.puntos + " puntos"
                              };
            ViewBag.ListPremios = new SelectList(listPremios, "id", "nombre");
            return View(canje_premios);
        }

        // GET: CanjePremios/Edit/5
        [Authorize(Roles = "CanjePremiosControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canje_premios canje_premios = await db.canje_premios.FindAsync(id);
            if (canje_premios == null)
            {
                return HttpNotFound();
            }
            return View(canje_premios);
        }

        // POST: CanjePremios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "CanjePremiosControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,socio_id,premio_id,puntos_canjeados,complejo_canje_id,fecha_alta,observaciones,premio_puntos_valor,premio_complejo_responsable_id,usuario_alta,fecha_modificacion,usaurio_modificacion")] canje_premios canje_premios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(canje_premios).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(canje_premios);
        }

        // GET: CanjePremios/Delete/5
        [Authorize(Roles = "CanjePremiosControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canje_premios canje_premios = await db.canje_premios.FindAsync(id);
            if (canje_premios == null)
            {
                return HttpNotFound();
            }
            return View(canje_premios);
        }

        // POST: CanjePremios/Delete/5
        [Authorize(Roles = "CanjePremiosControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            canje_premios canje_premios = await db.canje_premios.FindAsync(id);
            db.canje_premios.Remove(canje_premios);
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


        [HttpPost]
        public ActionResult SocioTarjeta(string nroTarjeta)
        {

            var socio = (from ts in db.tarjeta_socio
                         join s in db.socios on ts.socio_id equals s.id
                         where ts.numero_tarjeta == nroTarjeta && ts.fecha_baja == null && s.fecha_baja == null
                         select new { s.nombre, s.apellido, s.documento, s.puntos_actuales, s.observaciones, ts.numero_tarjeta, s.id }).FirstOrDefault();
            if (socio != null)
            {
                return Json(socio);
            }
            else
            {
                return Json("{}");
            }


        }

        public ActionResult SocioApeDoc(string term)
        {

            var socio = from ts in db.tarjeta_socio
                        join s in db.socios on ts.socio_id equals s.id
                        where (s.apellido.ToLower().Contains(term.ToLower()) || s.documento.Contains(term)) && ts.fecha_baja == null && s.fecha_baja == null
                        select new { s.nombre, s.apellido, s.documento, s.mail, s.observaciones, ts.numero_tarjeta, s.id };
            if (socio != null)
            {
                return Json(socio.Select(i => new jsonAutocomplete()
                {
                    label = i.apellido + ", " + i.nombre,
                    value = i.id.ToString()
                }).ToList());
            }
            else
            {
                return Json("{}");
            }


        }

        [HttpPost]
        public ActionResult SocioId(int idSocio)
        {

            var socio = (from ts in db.tarjeta_socio
                         join s in db.socios on ts.socio_id equals s.id
                         where s.id == idSocio && ts.fecha_baja == null && s.fecha_baja == null
                         select new { s.nombre, s.apellido, s.documento, s.puntos_actuales, s.observaciones, ts.numero_tarjeta, s.id }).FirstOrDefault();
            if (socio != null)
            {
                return Json(socio);
            }
            else
            {
                return Json("{}");
            }


        }
        [HttpPost]
        public ActionResult ValidarCanjePremio(int idSocio, int idPremio)
        {
            try
            {
                var so = db.socios.Find(idSocio);
                var puntosSocio = so.puntos_actuales;
                var puntosPremio = db.premios.Find(idPremio).puntos;
                jsonValidarCanje vc = new jsonValidarCanje();
                vc.PuntosPremio = puntosPremio;
                vc.PuntosSocio = puntosSocio;
                vc.NombreSocio = so.apellido + ", " + so.nombre;
                if (puntosSocio >= puntosPremio)
                {
                    vc.ValidacionOk = true;
                }
                else
                {
                    vc.ValidacionOk = false;
                }
                return Json(vc);
            }
            catch (Exception)
            {

                return Json("{}");
            }





        }
        public class jsonAutocomplete
        {
            public string label { get; set; }
            public string value { get; set; }
        }
        public class jsonValidarCanje
        {
            public string NombreSocio { get; set; }
            public Decimal PuntosSocio { get; set; }
            public decimal PuntosPremio { get; set; }
            public bool ValidacionOk { get; set; }
        }
    }
}
