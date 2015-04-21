using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Checkout.DevCon.Startup))]

namespace Checkout.DevCon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
