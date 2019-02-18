using System.Threading.Tasks;
using System.Web.Mvc;
using AdventureWorks.Web.Helpers;

namespace AdventureWorks.Web.Controllers
{
    [Authorize]
    public class PersonController : BaseController
    {
        public PersonController(IWebApiProxy webApiProxy) : base(webApiProxy)
        {
        }
        
        public async Task<ActionResult> ViewPerson(int id)
        {
            var result = await WebApiProxy.GetPerson(id);

            return View(result.Data);
        }
    }
}