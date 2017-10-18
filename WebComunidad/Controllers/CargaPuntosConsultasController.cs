using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebComunidad.Models;

namespace WebComunidad.Controllers
{
    public class CargaPuntosConsultasController : Controller
    {

        ComunidadContext db = new ComunidadContext();
        // GET: CargaPuntosConsultas
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "CargaPuntosConsultasControllerTotales")]
        [HttpGet]
        public ActionResult Totales()
        {
            return View();
        }


        [Authorize(Roles = "CargaPuntosConsultasControllerTotales")]
        [HttpPost]
        public ActionResult Totales(DateTime fechaDesde, DateTime fechaHasta)
        {
            DateTime fd = Helper.Helper.FechaHoraDesde(fechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(fechaHasta);
            Models.CargaPuntosConsulta.TotalesModels cp = new Models.CargaPuntosConsulta.TotalesModels();
            cp.FechaDesde = fechaDesde;
            cp.FechaHasta = fechaHasta;
            try
            {
                var totalPuntos = db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Sum(c => c.puntos_cargados);
                var totalDinero = db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Sum(c => c.monto_cargado);               
                cp.MontoEnPesosTotal = totalDinero;
                cp.TotalPuntosCargados = (int)totalPuntos;

            }
            catch (InvalidOperationException)
            {

            }
            return View(cp);
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