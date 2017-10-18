using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebComunidad.Startup))]
namespace WebComunidad
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
