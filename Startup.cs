using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Inventario360.Web.Startup))]
namespace Inventario360.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
