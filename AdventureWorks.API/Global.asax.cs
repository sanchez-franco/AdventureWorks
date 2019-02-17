using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AdventureWorks.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            HttpConfiguration config = GlobalConfiguration.Configuration;

            config.Formatters.JsonFormatter
                .SerializerSettings
                .ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}
