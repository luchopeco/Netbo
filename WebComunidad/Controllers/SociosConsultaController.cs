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

        [Authorize(Roles = "SociosConsultaControllerRankingCargas")]
        public ActionResult RankingCargas()
        {
            return View();
        }

        [Authorize(Roles = "SociosConsultaControllerRankingCargas")]
        [HttpPost]
        public ActionResult RankingCargas(DateTime fechaDesde, DateTime fechaHasta, int cantidadSocios)
        {

            fechaDesde = Helper.Helper.FechaHoraDesde(fechaDesde);
            fechaHasta = Helper.Helper.FechaHoraHasta(fechaHasta);


            var ran = from c in db.carga_puntos
                      where c.fecha_alta >= fechaDesde && c.fecha_alta <= fechaHasta
                      group c.socio by c.socio_id into soc
                      select new { listSocio = soc };
            List<Models.SociosConsulta.RankingCargasModels> listSocios = new List<Models.SociosConsulta.RankingCargasModels>();
            foreach (var item in ran)
            {
                var cc = item;
                Models.SociosConsulta.RankingCargasModels r = new Models.SociosConsulta.RankingCargasModels();
                r.FechaDesde = fechaDesde;
                r.FechaHasta = fechaHasta;
                r.IdSocio = cc.listSocio.First().id;
                r.ApenomSocio = cc.listSocio.First().apellido + ", " + cc.listSocio.First().nombre;
                r.Celular = cc.listSocio.First().celular;
                r.Documento = cc.listSocio.First().documento;
                r.Obs = cc.listSocio.First().observaciones;
                r.CantidadCargas = cc.listSocio.First().carga_puntos.Count();
                if (cc.listSocio.First().complejo != null)
                {
                    r.ComplejoAlta = cc.listSocio.First().complejo.descripcion;
                    r.ComplejoUltimaCarga = cc.listSocio.First().carga_puntos.OrderByDescending(c => c.fecha_alta).First().complejo.descripcion;
                }
                r.FechaUltimaCarga = cc.listSocio.First().carga_puntos.OrderByDescending(c => c.fecha_alta).First().fecha_alta;
                r.PuntosActuales = (int)cc.listSocio.First().puntos_actuales;
                listSocios.Add(r);
            }
            return View(listSocios);
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