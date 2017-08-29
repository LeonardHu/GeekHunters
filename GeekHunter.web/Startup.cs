using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeekHunter.Startup))]
namespace GeekHunter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
