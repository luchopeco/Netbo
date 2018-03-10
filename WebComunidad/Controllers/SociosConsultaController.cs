using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebComunidad.Models;

namespace WebComunidad.Controllers
{
    public class SociosConsultaController : Controller
    {
        ComunidadContext db = new ComunidadContext();
        // GET: SociosConsulta
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SociosConsultaControllerTotales")]
        [HttpGet]
        public ActionResult Totales()
        {
            return View();
        }


        [Authorize(Roles = "SociosConsultaControllerTotales")]
        [HttpPost]
        public ActionResult Totales(DateTime fechaDesde, DateTime fechaHasta)
        {
            DateTime fd = Helper.Helper.FechaHoraDesde(fechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(fechaHasta);
            Models.SociosConsulta.TotalesModels cp = new Models.SociosConsulta.TotalesModels();
            cp.FechaDesde = fechaDesde;
            cp.FechaHasta = fechaHasta;
            try
            {
                var cantidadSocios = db.socios.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Count();                
                cp.CantidadSocios = cantidadSocios;               
                var listC = db.complejoes;

                foreach (var cc in listC)
                {
                    Models.SociosConsulta.TotalPorComplejoModels comp = new Models.SociosConsulta.TotalPorComplejoModels();
                    comp.Complejo = cc.descripcion;
                    comp.ComplejoId = cc.id;
                    try
                    {
                        comp.CantidadSocios = db.socios.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh && c.complejo_alta_id == cc.id).Count();
                       
                        decimal aux = (decimal)comp.CantidadSocios / (decimal)cp.CantidadSocios;
                        comp.PorcentajeCantidadSocios = aux * 100;
                        
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