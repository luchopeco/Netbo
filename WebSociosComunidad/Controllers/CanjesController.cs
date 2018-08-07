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
    public class CanjesController : Controller
    {

        private static string UrlImagen
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UrlImagen"];
            }
        }
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
        // GET: Canjes
        public ActionResult Index()
        {
            Models.Canjes.index m = GenerarModeloIndex();
            return View(m);
           
        }

        [Authorize]
        // GET: Canjes
        public ActionResult AlAlcance()
        {
            var puntos = db.socios.Find(IdSocioIdentity).puntos_actuales;
            var p = db.premios.Where(pp=>pp.puntos<= puntos && pp.desactivado==false);
            List<Models.Canjes.AlAlcance> listP = new List<Models.Canjes.AlAlcance>();
            foreach (var item in p)
            {
                Models.Canjes.AlAlcance aa = new Models.Canjes.AlAlcance();
                aa.Premio = item.nombre;
                aa.Puntos = (int)item.puntos;
                aa.Alcance = true;   
                aa.UrlImagen = UrlImagen + item.UrlImagen;
                listP.Add(aa);
            }
            var listTodos = db.premios.Where(pp => pp.desactivado == false);
            foreach (var item in listTodos)
            {
                Models.Canjes.AlAlcance aa = new Models.Canjes.AlAlcance();
                aa.Premio = item.nombre;
                aa.Puntos = (int)item.puntos;
                aa.Alcance = false;
                aa.UrlImagen = UrlImagen + item.UrlImagen;
                listP.Add(aa);
            }

            return View(listP);

        }

        [Authorize]
        [HttpPost]
        public ActionResult PorFecha(Models.Canjes.index m)
        {
            Models.Canjes.index model = GenerarModeloIndex();
            model.FechaDesde = Helper.Helper.FechaHoraDesde((DateTime)m.FechaDesde);
            model.FechaHasta = Helper.Helper.FechaHoraHasta((DateTime)m.FechaHasta);
            var listCanjes = db.canje_premios.Where(c => c.socio_id == IdSocioIdentity && c.fecha_alta <= model.FechaHasta && c.fecha_alta >= model.FechaDesde);
            foreach (var item in listCanjes)
            {
                Models.Canjes.CanjesPremios cp = new Models.Canjes.CanjesPremios();
                cp.FechaCanje = item.fecha_alta;
                cp.PuntosCanjeados = (int)item.puntos_canjeados;
                cp.Premio = item.premio.nombre;
                cp.ComplejoDesc = item.complejo.descripcion;
                model.ListcanjesBusqueda.Add(cp);
            }
            if (listCanjes == null || listCanjes.Count() == 0)
            {
                ViewBag.MsjError = "No se encontraron canjes de premios para la fecha indicada";
            }
            return View("Index", model);
        }
        private Models.Canjes.index GenerarModeloIndex()
        {
            Models.Canjes.index m = new Models.Canjes.index();
            m.TotalCanjes = (int)db.canje_premios.Count(s=>s.socio_id==IdSocioIdentity);       
            DateTime fechaHasta = Helper.Helper.FechaHoraHasta(DateTime.Today.AddDays(-30));
            DateTime fechaDesder = Helper.Helper.FechaHoraDesde(DateTime.Today);
            var listCanjes30Dias = db.canje_premios.Where(c => c.socio_id == IdSocioIdentity && c.fecha_alta <= fechaDesder && c.fecha_alta >= fechaHasta);
            foreach (var item in listCanjes30Dias)
            {
                Models.Canjes.CanjesPremios cp = new Models.Canjes.CanjesPremios();
                cp.FechaCanje = item.fecha_alta;
                cp.PuntosCanjeados = (int)item.puntos_canjeados;
                cp.Premio = item.premio.nombre;
                cp.ComplejoDesc = item.complejo.descripcion;
                m.ListCanjesUltimoMes.Add(cp);
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