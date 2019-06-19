using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TransafeRx.Startup))]
namespace TransafeRx
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
