using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SandwhichMVC.Startup))]
namespace SandwhichMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
