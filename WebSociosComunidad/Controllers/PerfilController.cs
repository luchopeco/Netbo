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
    public class PerfilController : Controller
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
        // GET: Perfil
        public ActionResult Index()
        {
            var s = db.socios.Find(IdSocioIdentity);
            if (s.complejo_alta_id !=null)
            {
                ViewBag.Complejo = s.complejo.descripcion;
            }
           
            return View(s);

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