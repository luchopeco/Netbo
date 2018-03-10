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
    public class HomeController : Controller
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
            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData["MsjExito"] = null;
            }

            Models.HomeModels hm = new Models.HomeModels();
            hm.CantidadPremiosCanjeados = db.canje_premios.Count(c=>c.socio_id==IdSocioIdentity);
            hm.PuntosDisponibles = (int)db.socios.Find(IdSocioIdentity).puntos_actuales;
            try
            {
                hm.UltimaCargaDePuntos = db.carga_puntos.Where(s => s.socio_id == IdSocioIdentity).Max(c => c.fecha_alta).ToShortDateString();
            }
            catch (InvalidOperationException)
            {
                hm.UltimaCargaDePuntos = "Sin Cargas";
            }
            try
            {
                hm.UltimoPremioCanjeado = db.canje_premios.Where(c => c.socio_id == IdSocioIdentity).OrderByDescending(c => c.fecha_alta).First().premio.nombre;
            }
            catch (InvalidOperationException)
            {
                hm.UltimoPremioCanjeado = "Sin Premios";
            }
            
            
            return View(hm);
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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