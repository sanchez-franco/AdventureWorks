using System.Web.Http;
using AdventureWorks.Business.Person;

namespace AdventureWorks.API.Controllers
{
    [AuthorizeAttribute]
    [RoutePrefix("person")]
    public class PersonController : ApiController
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Route("addresses/{personId:int}")]
        public IHttpActionResult GetPersonAddresses(int personId)
        {
            return Ok(_personService.GetPersonAddresses(personId));
        }
    }
}