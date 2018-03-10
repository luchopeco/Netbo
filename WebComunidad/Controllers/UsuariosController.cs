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
using WebComunidad.Models;

namespace WebComunidad.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {

        private ComunidadContext db = new ComunidadContext();

        // GET: AspNetUsers
        [Authorize(Roles = "UsuariosControllerIndex")]
        public async Task<ActionResult> Index()
        {
            return View(await db.AspNetUsers.ToListAsync());
        }

        /// <summary>
        /// Muestra usuarios que no tienen el perfil supersu
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "UsuariosControllerFinales")]
        public async Task<ActionResult> Finales()
        {
            if (TempData["MsjError"] != null)
            {
                ViewBag.MsjError = TempData["MsjError"];
                TempData.Remove("MsjError");
            }
            ViewBag.Finales = "1";
            return View("Index", await db.AspNetUsers.Where(u => u.AspNet_Perfiles.Count(p => p.descripcion == "Super Usuario") == 0).ToListAsync());
        }

        /// <summary>
        /// Muestra usuarios que no tienen el perfil supersu
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "UsuariosControllerEncargado")]
        public async Task<ActionResult> Encargado()
        {
            if (TempData["MsjError"] != null)
            {
                ViewBag.MsjError = TempData["MsjError"];
                TempData.Remove("MsjError");
            }
            ViewBag.Finales = "2";
            return View("Index", await db.AspNetUsers.Where(u => u.AspNet_Perfiles.Count(p => p.descripcion == "Super Usuario") == 0 &&  u.AspNet_Perfiles.Count(p => p.descripcion == "Administrador") == 0).ToListAsync());
        }
        // GET: AspNetUsers/Details/5
        [Authorize(Roles = "UsuariosControllerDetails")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Create
        [Authorize(Roles = "UsuariosControllerCreate")]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "UsuariosControllerCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        [Authorize(Roles = "UsuariosControllerEdit")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            ViewBag.ListComplejos = new SelectList(
                 db.complejoes.Where(cc => cc.fecha_baja == null),
                 "id", "descripcion");
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "UsuariosControllerEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,complejo_id")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                var usu = db.AspNetUsers.Find(aspNetUser.Id);
                usu.complejo_id = aspNetUser.complejo_id;
                db.Entry(usu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Encargado");
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        [Authorize(Roles = "UsuariosControllerDelete")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [Authorize(Roles = "UsuariosControllerDeleteConfirmed")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Encargado");
        }


        [Authorize(Roles = "UsuariosControllerPerfiles")]
        public async Task<ActionResult> Perfiles(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            var perfiles = await db.AspNet_Perfiles.ToListAsync();

            foreach (var cont in perfiles)
            {
                if (aspNetUser.AspNet_Perfiles.ToList().Exists(rr => rr.id == cont.id))
                {
                    cont.Activo = true;
                }
            }
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }

            ViewBag.IdUsuario = aspNetUser.Id;
            ViewBag.Usuario = aspNetUser.UserName;

            return View(perfiles);
        }


        /// <summary>
        /// Muestra los perfiles para un usuario que no sean supersu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "UsuariosControllerPerfilesFinales")]
        public async Task<ActionResult> PerfilesFinales(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            var perfiles = await db.AspNet_Perfiles.Where(p => p.descripcion != "Super Usuario").ToListAsync();
            foreach (var cont in perfiles)
            {
                if (aspNetUser.AspNet_Perfiles.ToList().Exists(rr => rr.id == cont.id))
                {
                    cont.Activo = true;
                }
            }
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }

            ViewBag.IdUsuario = aspNetUser.Id;
            ViewBag.Usuario = aspNetUser.UserName;

            return View("Perfiles",perfiles);
        }

        /// <summary>
        /// Muestra los perfiles para un usuario que no sean supersu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "UsuariosControllerPerfilesEncargado")]
        public async Task<ActionResult> PerfilesEncargado(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            var perfiles = await db.AspNet_Perfiles.Where(p => p.descripcion != "Super Usuario" && p.descripcion != "Administrador").ToListAsync();
            foreach (var cont in perfiles)
            {
                if (aspNetUser.AspNet_Perfiles.ToList().Exists(rr => rr.id == cont.id))
                {
                    cont.Activo = true;
                }
            }
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            if (TempData["MsjExito"] != null)
            {
                ViewBag.MsjExito = TempData["MsjExito"];
                TempData.Remove("MsjExito");
            }

            ViewBag.IdUsuario = aspNetUser.Id;
            ViewBag.Usuario = aspNetUser.UserName;

            return View("Perfiles", perfiles);
        }

        [Authorize(Roles = "UsuariosControllerPerfiles")]
        [HttpPost]
        public async Task<ActionResult> Perfiles(List<AspNet_Perfiles> listPerfiles)
        {
            var idUsario = Request.Form["id-usuario"];
            Entidades.EF.AspNetUser usu = new AspNetUser();
            usu.Id = idUsario;
            foreach (var p in listPerfiles)
            {
                if (p.Activo)
                {
                    usu.AspNet_Perfiles.Add(p);
                }
            }
            Negocio.ControladorUsuarios.AgregarPerfilesUsuario(usu);
            TempData["MsjExito"] = "Perfiles Modificados Correctamente";
            return RedirectToAction("PerfilesEncargado", new { id = idUsario });
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
