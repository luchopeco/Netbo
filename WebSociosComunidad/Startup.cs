using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSociosComunidad.Startup))]
namespace WebSociosComunidad
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
