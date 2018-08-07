using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebComunidad.Models;

namespace WebComunidad.Controllers
{
    public class CanjePremiosConsultasController : Controller
    {
        ComunidadContext db = new ComunidadContext();

        // GET: CanjePuntosConsultas
        [Authorize(Roles = "CanjePremiosConsultasControllerIndex")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "CanjePremiosConsultasControllerPorComplejo")]
        public ActionResult PorComplejo()
        {

            ViewBag.ListComplejos = new SelectList(
                 db.complejoes.Where(cc => cc.fecha_baja == null),
                 "id", "descripcion");

            return View();
        }

        [Authorize(Roles = "CanjePremiosConsultasControllerPorComplejo")]
        [HttpPost]
        public ActionResult PorComplejo(Models.CanjePuntosConsultas.PorComplejoModels dc)
        {
            ViewBag.ListComplejos = new SelectList(
                db.complejoes.Where(cc => cc.fecha_baja == null),
                "id", "descripcion");

            DateTime fechaDesde = new DateTime(dc.FechaDesde.Year, dc.FechaDesde.Month, dc.FechaDesde.Day, 0, 0, 0);
            DateTime fechaHasta = new DateTime(dc.FechaHasta.Year, dc.FechaHasta.Month, dc.FechaHasta.Day, 23, 59, 59);
            var canjes = db.canje_premios.Where(cp => cp.complejo_canje_id == dc.IdComplejo && cp.fecha_alta >= fechaDesde && cp.fecha_alta <= fechaHasta);
            dc.ListCanjes = canjes.ToList();
            return View(dc);
        }

        // GET: Socios/Details/5
        [Authorize(Roles = "CanjePremiosConsultasControllerDetalle")]
        [HttpGet]
        public async Task<ActionResult> Detalle(int id)
        {
            var canje = await db.canje_premios.FindAsync(id);
            return View(canje);
        }

        [Authorize(Roles = "CanjePremiosConsultasControllerTotalesPorComplejo")]
        [HttpGet]
        public ActionResult TotalesPorComplejo()
        {
            return View();
        }
        [Authorize(Roles = "CanjePremiosConsultasControllerTotalesPorComplejo")]
        [HttpPost]
        public ActionResult TotalesPorComplejo(DateTime fechaDesde, DateTime fechaHasta)
        {
            WebComunidad.Models.CanjePuntosConsultas.TotalesPorComplejoModels totales = new Models.CanjePuntosConsultas.TotalesPorComplejoModels();

            DateTime fd = Helper.Helper.FechaHoraDesde(fechaDesde);
            DateTime fh = Helper.Helper.FechaHoraHasta(fechaHasta);

            ViewBag.SubTitulo = "Desde el " + fd.ToShortDateString() + " hasta el " + fh.ToShortDateString();

            int cantidad = db.carga_puntos.Count(c => c.fecha_alta >= fd && c.fecha_alta <= fh);
            if (cantidad > 0)
            {


                var tot = from c in db.canje_premios
                          where c.fecha_alta >= fd && c.fecha_alta <= fh
                          group c by c.complejo_canje_id into grupoCanje
                          select new { grupoCanje };

                var puntosCargadosPorComplejos = from cp in db.carga_puntos
                                                 where cp.fecha_alta >= fd && cp.fecha_alta <= fh
                                                 group cp by cp.complejo_id into grupoCarga
                                                 select new { grupoCarga };

                decimal totalPuntosCargados = db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Sum(c => c.puntos_cargados);
                decimal totalCargaPuntos = db.carga_puntos.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Count();
                decimal totalPuntosCanjeados = db.canje_premios.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Sum(c => c.puntos_canjeados);
                decimal totalCanjes = db.canje_premios.Where(c => c.fecha_alta >= fd && c.fecha_alta <= fh).Count();

                totales.TotalPuntosCargados = totalPuntosCargados.ToString();
                totales.TotalPuntosCanjeados = totalPuntosCanjeados.ToString();
                totales.TotalPremiosCanjeados = totalCanjes.ToString();

                var listPremio = from cp in db.canje_premios
                             where cp.fecha_alta >= fechaDesde && cp.fecha_alta<=fechaHasta
                                 group cp by cp.premio into grupoPremios
                                 select grupoPremios;
                foreach (var p in listPremio)
                {
                    Models.CanjePuntosConsultas.TotalesPorComplejoPremiosModels tcp = new Models.CanjePuntosConsultas.TotalesPorComplejoPremiosModels();
                    tcp.Premio = p.FirstOrDefault().premio.nombre;
                    tcp.CantidadCanjeada = p.Count();
                    totales.ListPremiosTotales.Add(tcp);
                }

                foreach (var t in tot)
                {
                    Models.CanjePuntosConsultas.TotalesDiscriminadoPorComplejoModels tc = new Models.CanjePuntosConsultas.TotalesDiscriminadoPorComplejoModels();
                    tc.IdComplejo = t.grupoCanje.Key;
                    tc.Complejo = db.complejoes.Find(t.grupoCanje.Key).descripcion;
                    tc.TotalPuntosCanjeados = t.grupoCanje.Sum(c => c.puntos_canjeados);
                    tc.TotalCanjesRealizados = t.grupoCanje.Count();
                    tc.PorcentajeCanjesDelTotal = (tc.TotalCanjesRealizados * 100) / totalCanjes;
                    tc.PorcentajePuntosDelTotal = (tc.TotalPuntosCanjeados * 100) / totalPuntosCanjeados;


                    tc.ListPremios = new List<Models.CanjePuntosConsultas.TotalesPorComplejoPremiosModels>();
                    var premio = from p in db.canje_premios
                                 where p.complejo_canje_id == t.grupoCanje.Key
                                 group p by p.premio_id into grupoPremio
                                 select grupoPremio;
                    foreach (var p in premio)
                    {
                        Models.CanjePuntosConsultas.TotalesPorComplejoPremiosModels tcp = new Models.CanjePuntosConsultas.TotalesPorComplejoPremiosModels();
                        tcp.Premio = db.premios.Find(p.Key).nombre;
                        tcp.CantidadCanjeada = p.Count();
                        tc.ListPremios.Add(tcp);
                    }

                    totales.ListTotalesPorComplejo.Add(tc);
                }
                foreach (var pc in puntosCargadosPorComplejos)
                {
                    if (totales.ListTotalesPorComplejo.Exists(t => t.IdComplejo == pc.grupoCarga.Key))
                    {
                        totales.ListTotalesPorComplejo.FirstOrDefault(t => t.IdComplejo == pc.grupoCarga.Key).TotalPuntosCargados = pc.grupoCarga.Sum(p => p.puntos_cargados);
                        totales.ListTotalesPorComplejo.FirstOrDefault(t => t.IdComplejo == pc.grupoCarga.Key).PorcentajePuntosCargadosDelTotal = (totales.ListTotalesPorComplejo.FirstOrDefault(t => t.IdComplejo == pc.grupoCarga.Key).TotalPuntosCargados * 100) / totalPuntosCargados;
                    }
                    else
                    {
                        Models.CanjePuntosConsultas.TotalesDiscriminadoPorComplejoModels tc = new Models.CanjePuntosConsultas.TotalesDiscriminadoPorComplejoModels();
                        tc.IdComplejo = pc.grupoCarga.Key;
                        tc.Complejo = db.complejoes.Find(pc.grupoCarga.Key).descripcion;
                        tc.TotalPuntosCargados = pc.grupoCarga.Sum(p => p.puntos_cargados);
                        tc.PorcentajePuntosCargadosDelTotal = (tc.TotalPuntosCargados * 100) / totalPuntosCargados;
                        totales.ListTotalesPorComplejo.Add(tc);

                    }

                }
            }

            return View(totales);
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