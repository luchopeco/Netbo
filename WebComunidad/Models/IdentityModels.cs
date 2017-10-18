using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebComunidad.Models
{
    // Puede agregar datos del perfil del usuario agregando más propiedades a la clase ApplicationUser. Para más información, visite http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Agregar aquí notificaciones personalizadas de usuario
            ComunidadContext db = new ComunidadContext();
            Entidades.EF.AspNetUser u = db.AspNetUsers.Find(this.Id);
            int? idComplejo = u.complejo_id;
            string complejo = "";
            if (idComplejo != null)
            {
                complejo = u.complejo.descripcion;
            }
            else
            {
                idComplejo = 0;
            }

            //IdentityUserClaim cl = new IdentityUserClaim();
            //cl.ClaimType = "Complejo";
            //cl.ClaimValue = complejo;
            //IdentityUserClaim cl1 = new IdentityUserClaim();
            //cl1.ClaimType = "IdComplejo";
            //cl1.ClaimValue = idComplejo.ToString();

            Claim cll = new Claim("Complejo",complejo);
            Claim cll1 = new Claim("IdComplejo", idComplejo.ToString());
            userIdentity.AddClaim(cll);
            userIdentity.AddClaim(cll1);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }



    }
}