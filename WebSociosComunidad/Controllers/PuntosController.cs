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
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using WebSociosComunidad.Models;

namespace WebSociosComunidad.Controllers
{
    public class PuntosController : Controller
    {
        private ComunidadContext db = new ComunidadContext();
        public int IdSocioIdentity
        {
            get
            {
                return Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirstValue("IdSocio"));
            }
        }
        public int NombreSocioIdentity
        {
            get
            {
                return Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirstValue("NombreSocio"));
            }
        }

        [Authorize]
        public ActionResult Index()
        {
            Models.Puntos.index m = GenerarModeloIndex();
            return View(m);
        }
        [Authorize]
        [HttpPost]
        public ActionResult PorFecha(Models.Puntos.index m)
        {
            Models.Puntos.index model = GenerarModeloIndex();
            model.FechaDesde = Helper.Helper.FechaHoraDesde((DateTime)m.FechaDesde);
            model.FechaHasta = Helper.Helper.FechaHoraHasta((DateTime)m.FechaHasta);
            var listCargas = db.carga_puntos.Where(c => c.socio_id == IdSocioIdentity && c.fecha_alta <= model.FechaHasta && c.fecha_alta >= model.FechaDesde);
            foreach (var item in listCargas)
            {
                Models.Puntos.CargasPuntos cp = new Models.Puntos.CargasPuntos();
                cp.FechaCarga = item.fecha_alta;
                cp.Puntos = (int)item.puntos_cargados;
                cp.TipoBeneficioDesc = item.tipo_beneficio.descripcion;
                cp.ComplejoDesc = item.complejo.descripcion;
                model.ListCargasBusqueda.Add(cp);
            }
            if (listCargas==null || listCargas.Count()==0)
            {
                ViewBag.MsjError = "No se encontraron cargas de puntos para la fecha indicada";
            }
            return View("Index",model);
        }

        private Models.Puntos.index GenerarModeloIndex()
        {
            Models.Puntos.index m = new Models.Puntos.index();
            m.TotalPuntos = (int)db.socios.Find(IdSocioIdentity).puntos_actuales;
            DateTime fechaHasta = Helper.Helper.FechaHoraHasta(DateTime.Today.AddDays(-30));
            DateTime fechaDesde = Helper.Helper.FechaHoraDesde(DateTime.Today);
            var listCargas30Dias = db.carga_puntos.Where(c => c.socio_id == IdSocioIdentity && c.fecha_alta <= fechaDesde && c.fecha_alta >= fechaHasta);
            foreach (var item in listCargas30Dias)
            {
                Models.Puntos.CargasPuntos cp = new Models.Puntos.CargasPuntos();
                cp.FechaCarga = item.fecha_alta;
                cp.Puntos = (int)item.puntos_cargados;
                cp.TipoBeneficioDesc = item.tipo_beneficio.descripcion;
                cp.ComplejoDesc = item.complejo.descripcion;
                m.ListCargasUltimoMes.Add(cp);
            }

            return m;
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