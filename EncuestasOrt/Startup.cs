using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EncuestasOrt.Startup))]
namespace EncuestasOrt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
