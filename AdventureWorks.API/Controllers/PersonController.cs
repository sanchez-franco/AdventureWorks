using System.Net;
using System.Web.Http;
using AdventureWorks.Business.Person;
using AdventureWorks.Common;

namespace AdventureWorks.API.Controllers
{
    [Authorize]
    [RoutePrefix("person")]
    public class PersonController : ApiController
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Route("{personId:int}")]
        public IHttpActionResult GetPerson(int personId)
        {
            var retValue = new Result<PersonDetail>
            {
                Data = _personService.GetPerson(personId)
            };
            retValue.Status = retValue.Data != null ? HttpStatusCode.OK : HttpStatusCode.NotFound;

            return Ok(retValue);
        }

        [HttpGet]
        [Route("addresses/{personId:int}")]
        public IHttpActionResult GetPersonAddresses(int personId)
        {
            return Ok(_personService.GetPersonAddresses(personId));
        }
    }
}