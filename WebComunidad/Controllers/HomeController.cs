using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Entidades.EF;
using WebComunidad.rdlc;

namespace WebComunidad.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {

           if (TempData["MsjError"] !=null)
            {
                ViewBag.MsjError = TempData["MsjError"];
            }
            //Entidades.MDD.DatosPersona dp=new DatosApoderadoPersonaJuridica();
            // //dp.Apellido = "EFXDEMOTA";
            // //dp.Nombre = "ALFREDO";
            // //dp.Documento = 87437433;
            // dp.Apellido = "Cosentino";
            // dp.Nombre = "Leonardo";
            // dp.Documento = 31787266;
            // dp.Sexo = "M";
            // dp.FechaNacimiento = DateTime.Today.AddYears(-20);
            // AccesoVeraz.Veraz av=new Veraz();
            // av.SolicitudVeraz(dp,20000,100000,48);

            //MailMessage email = new MailMessage();
            //email.To.Add(new MailAddress("luchopeco@gmail.com"));
            //email.From = new MailAddress("procesos-sbi@sancristobalsf.com.ar");
            //email.Subject = "Asunto ";
            //email.Body = "zarazasadasdads.";
            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;

            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "mail.sancristobalcaja.com.ar";
            //smtp.Port = 25;
            //smtp.EnableSsl = false;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("procesos-sbi@sancristobalsf.com.ar", "nelnuevoxRAN");

            //smtp.Send(email);
            //email.Dispose();
            
            

            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}