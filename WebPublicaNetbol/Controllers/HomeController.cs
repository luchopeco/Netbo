using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPublicaNetbol.Models;

namespace WebPublicaNetbol.Controllers
{
    public class HomeController : Controller
    {

        private static string UrlImagen
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UrlImagen"];
            }
        }

        private ComunidadContext db = new ComunidadContext();
        public ActionResult Index()
        {
            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }
            if (TempData["MsjError"] != null)
            {
                ViewBag.MsjError = TempData["MsjError"];
                TempData.Remove("MsjError");
            }
            ViewBag.UrlImagen = UrlImagen;
            var premios = db.premios.Where(p=>p.desactivado==false);
            return View(premios);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contacto(string nombre, string mail, string ciudad, string mensaje)
        {
            try
            {
                var smtp = Helper.MailSender.GetSmtpClient();
                var from = Helper.MailSender.GetFrom();
                var to = Helper.MailSender.GetAddresses(Helper.MailSender.AddresseType.To);
                string asunto = "Consulta desde la web";
                string body = "<p>Nombre: " + nombre + "</p>" + "<p>Mail: " + mail + "</p>"
                    + "<p>Ciudad: " + ciudad + "</p>" + "<p>Mensaje: " + mensaje + "</p>";
                var res = Helper.MailSender.SendMail(smtp, from, to, asunto, body, true);

                TempData["MsjExito"] = "Mensaje enviado con exito. Pronto nos pondremos en contacto.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MsjError"] = "Disculpá, no pudimos enviar la consulta. Llamanos o intentá mas tarde.";
                return RedirectToAction("Index");
            }
           
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