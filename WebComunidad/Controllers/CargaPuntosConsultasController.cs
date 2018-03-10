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
                var cantidadCargas = db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Count();
                cp.MontoEnPesosTotal = totalDinero;
                cp.TotalPuntosCargados = (int)totalPuntos;
                cp.CantidadCargasRealizadas = cantidadCargas;
                var listC = db.complejoes;

                foreach (var cc in listC)
                {
                    Models.CargaPuntosConsulta.TotalPorComplejoModels comp = new Models.CargaPuntosConsulta.TotalPorComplejoModels();
                    comp.Complejo = cc.descripcion;
                    comp.ComplejoId = cc.id;
                    try
                    {
                        comp.MontoEnPesosTotal = db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh && c.complejo_id == cc.id).Sum(c => c.monto_cargado);
                        comp.TotalPuntosCargados = (int)db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh && c.complejo_id == cc.id).Sum(c => c.puntos_cargados);
                        comp.CantidadCargasRealizadas = db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh && c.complejo_id == cc.id).Count();

                        decimal aux = (decimal)comp.TotalPuntosCargados / (decimal)cp.TotalPuntosCargados;
                        comp.PorcentajePuntosCargados = aux* 100;

                        decimal auxDinero = (decimal)comp.MontoEnPesosTotal / (decimal)cp.MontoEnPesosTotal;
                        comp.PorcentajeMontoEnPesosCargado = auxDinero * 100;

                        decimal auxcargas = (decimal)comp.CantidadCargasRealizadas / (decimal)cp.CantidadCargasRealizadas;
                        comp.PorcentajeCantidadCargasRealizadas = auxcargas * 100;
                    }
                    catch (NullReferenceException)
                    {
                      
                    }
                    catch (InvalidOperationException)
                    {

                    }
                    cp.ListComplejos.Add(comp);
                }

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