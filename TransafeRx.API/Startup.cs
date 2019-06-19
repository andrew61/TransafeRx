using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TransafeRx.API.Startup))]

namespace TransafeRx.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
