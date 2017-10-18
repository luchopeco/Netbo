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
    public class CargaPuntosController : Controller
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
        /// <summary>
        /// Solo devuelve los del complejo en cuestion
        /// </summary>
        /// <returns></returns>
        // GET: CargaPuntos
        [Authorize(Roles = "CargaPuntosControllerIndex")]
        public async Task<ActionResult> Index()
        {
            Models.CargaPuntos.CargaPuntosIndexModels cp = new Models.CargaPuntos.CargaPuntosIndexModels();
            cp.FechaDesde = DateTime.Today;
            cp.FechaHasta = DateTime.Today;
            DateTime fd = Helper.Helper.FechaHoraDesde(DateTime.Today);
            DateTime fh = Helper.Helper.FechaHoraHasta(DateTime.Today);
            ViewBag.Complejo = ComplejoDescIdentity;
            cp.ListCargaPuntos = await db.carga_puntos.Where(c => c.complejo_id == IdComplejoIdentity && c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View(cp);
        }
        [Authorize(Roles = "CargaPuntosControllerTodas")]
        public async Task<ActionResult> Todas()
        {
            Models.CargaPuntos.CargaPuntosIndexModels cp = new Models.CargaPuntos.CargaPuntosIndexModels();
            cp.FechaDesde = DateTime.Today;
            cp.FechaHasta = DateTime.Today;
            DateTime fd = Helper.Helper.FechaHoraDesde(DateTime.Today);
            DateTime fh = Helper.Helper.FechaHoraHasta(DateTime.Today);
            ViewBag.Complejo = "Todos";
            cp.ListCargaPuntos = await db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View(cp);
        }

        [Authorize(Roles = "CargaPuntosControllerPorFecha")]
        [HttpPost]
        public async Task<ActionResult> PorFecha(Models.CargaPuntos.CargaPuntosIndexModels cp)
        {
            DateTime fd = Helper.Helper.FechaHoraDesde(cp.FechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(cp.FechaHasta);
            ViewBag.Complejo = ComplejoDescIdentity;
            cp.ListCargaPuntos = await db.carga_puntos.Where(c => c.complejo_id == IdComplejoIdentity && c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View("index", cp);
        }

        [Authorize(Roles = "CargaPuntosControllerPorFechaTodas")]
        [HttpPost]
        public async Task<ActionResult> PorFechaTodas(Models.CargaPuntos.CargaPuntosIndexModels cp)
        {
            DateTime fd = Helper.Helper.FechaHoraDesde(cp.FechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(cp.FechaHasta);
            ViewBag.Complejo = "Todos";
            cp.ListCargaPuntos = await db.carga_puntos.Where(c =>c.fecha_alta >= fd && c.fecha_alta <= fh).ToListAsync();
            return View("Todas", cp);
        }
        // GET: CargaPuntos/Details/5
        [Authorize(Roles = "CargaPuntosControllerDetails")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carga_puntos carga_puntos = await db.carga_puntos.FindAsync(id);
            if (carga_puntos == null)
            {
                return HttpNotFound();
            }
            return View(carga_puntos);
        }

        // GET: CargaPuntos/Create
        [Authorize(Roles = "CargaPuntosControllerCreate")]
        public ActionResult Create()
        {
            if (TempData["MsjError"] != null)
            {
                ViewBag.MsjError = TempData["MsjError"];
                TempData.Remove("MsjError");
            }
            ViewBag.ListTiposBeneficios = new SelectList(
               db.tipo_beneficio.Where(cc => cc.fecha_baja == null),
               "id", "descripcion");
            return View();
        }

        // POST: CargaPuntos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "CargaPuntosControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "socio_id,tipo_beneficio_id,monto_cargado,puntos_cargados,tipo_beneficio_monto,tipo_beneficio_puntos,observaciones")] carga_puntos carga_puntos)
        {
            var idSocio = 0;
            if (ModelState.IsValid)
            {
                carga_puntos.fecha_alta = DateTime.Now;
                carga_puntos.fecha_modificacion = DateTime.Now;
                carga_puntos.usuario_alta = User.Identity.Name;
                carga_puntos.usuario_modificacion = User.Identity.Name;
                carga_puntos.puntos_disponibles = carga_puntos.puntos_cargados;
                try
                {
                    carga_puntos.complejo_id = IdComplejoIdentity;
                    if (carga_puntos.complejo_id == 0)
                    {
                        TempData["MsjError"] = "Por algun motivo no se pudo recuperar el complejo en el cual se encuentra logueado. Por Favor Cierre Session e intente nuevamente.";
                        return RedirectToAction("Create");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");

                }

                var s = db.socios.Find(carga_puntos.socio_id);
                s.puntos_actuales = s.puntos_actuales + carga_puntos.puntos_cargados;
                db.Entry(s).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.carga_puntos.Add(carga_puntos);
                idSocio = s.id;
                await db.SaveChangesAsync();

            }
            TempData["MsjExito"] = "Puntos Cargados Correctamente";
            return RedirectToAction("PuntosCargados", "Socios", new { id = idSocio });
        }

        // GET: CargaPuntos/Edit/5
        [Authorize(Roles = "CargaPuntosControllerEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carga_puntos carga_puntos = await db.carga_puntos.FindAsync(id);
            if (carga_puntos == null)
            {
                return HttpNotFound();
            }
            return View(carga_puntos);
        }

        // POST: CargaPuntos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "CargaPuntosControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,socio_id,tipo_beneficio_id,monto_cargado,puntos_cargados,tipo_beneficio_monto,tipo_beneficio_puntos,observaciones,fecha_alta,usuario_alta,fecha_modificacion,usuario_modificacion")] carga_puntos carga_puntos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carga_puntos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(carga_puntos);
        }

        // GET: CargaPuntos/Delete/5
        [Authorize(Roles = "CargaPuntosControllerDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carga_puntos carga_puntos = await db.carga_puntos.FindAsync(id);
            if (carga_puntos == null)
            {
                return HttpNotFound();
            }
            return View(carga_puntos);
        }

        // POST: CargaPuntos/Delete/5
        [Authorize(Roles = "CargaPuntosControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            carga_puntos carga_puntos = await db.carga_puntos.FindAsync(id);
            db.carga_puntos.Remove(carga_puntos);
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
                         select new { s.nombre, s.apellido, s.documento, s.mail, s.observaciones, ts.numero_tarjeta, s.id, s.puntos_actuales }).FirstOrDefault();
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
        public ActionResult SocioId(int idSocio)
        {

            var socio = (from ts in db.tarjeta_socio
                         join s in db.socios on ts.socio_id equals s.id
                         where s.id == idSocio && ts.fecha_baja == null && s.fecha_baja == null
                         select new { s.nombre, s.apellido, s.documento, s.mail, s.observaciones, ts.numero_tarjeta, s.id, s.puntos_actuales }).FirstOrDefault();
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
                        select new { s.nombre, s.apellido, s.documento, s.mail, s.observaciones, ts.numero_tarjeta, s.id, s.puntos_actuales };
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


        public ActionResult ValidarCargaPunto(decimal monto, int idTipoBeneficio)
        {
            try
            {
                var tb = db.tipo_beneficio.FirstOrDefault(t => t.id == idTipoBeneficio);
                var puntosAcargar = (monto * tb.puntos) / tb.monto_dinero;
                int puntos = Convert.ToInt32(Decimal.Round(puntosAcargar));
                jsonValidarPuntos js = new jsonValidarPuntos();
                js.Puntos = puntos;
                js.TipoBeneficioMonto = tb.monto_dinero;
                js.TipoBeneficioPunto = tb.puntos;
                return Json(js);
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

        public class jsonValidarPuntos
        {
            public int Puntos { get; set; }
            public decimal TipoBeneficioPunto { get; set; }
            public decimal TipoBeneficioMonto { get; set; }
        }

    }
}
