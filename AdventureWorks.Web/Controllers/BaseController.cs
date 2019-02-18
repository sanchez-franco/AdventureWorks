using System.Security.Claims;
using System.Web.Mvc;
using AdventureWorks.Web.Helpers;

namespace AdventureWorks.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IWebApiProxy WebApiProxy;

        public BaseController(IWebApiProxy webApiProxy)
        {
            WebApiProxy = webApiProxy;
        }

        protected int Id
        {
            get
            {
                var claimsIdentity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;
                var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

                int id;
                int.TryParse(claim != null ? claim.Value : string.Empty, out id);

                return id;
            }
        }
    }
}