using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AdventureWorks.API.Startup))]

namespace AdventureWorks.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
        }
    }
}
