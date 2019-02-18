using System.Threading.Tasks;
using AdventureWorks.Common;

namespace AdventureWorks.Web.Helpers
{
    public interface IWebApiProxy
    {
        Task<Result<PersonDetail>> GetPerson(int personId);
        Task<Result<User>> ValidateUserPassword(string userName, string password);
    }

    public class WebApiProxy : IWebApiProxy
    {
        public async Task<Result<PersonDetail>> GetPerson(int personId)
        {
            var webApiClient = new WebApiClient<PersonDetail>();
            return await webApiClient.GetAsync("person", personId.ToString());
        }

        public async Task<Result<User>> ValidateUserPassword(string userName, string password)
        {
            var webApiClient = new WebApiClient<User>();
            return await webApiClient.AuthenticateAsync(userName, password);
        }
    }
}