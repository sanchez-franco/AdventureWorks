using AdventureWorks.Business.UnitOfWork;
using AdventureWorks.Common;

namespace AdventureWorks.Business.Authentication
{
    public interface IAuthenticationService
    {
        User ValidateUserPassword(string userName, string password);
    }

    public class AuthenticationService : BaseService, IAuthenticationService
    {
        public AuthenticationService(IUnitOfWorkProvider unitOfWorkProvider) : base(unitOfWorkProvider)
        {
        }

        public User ValidateUserPassword(string userName, string password)
        {
            using (var uow = UnitOfWorkProvider.GetAdventureWorksUnitOfWork())
            {
                User retValue = null;
                var person = uow.PersonRepository.GetPerson(userName, password);
                if (person != null)
                {
                    retValue = new User
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        UserId = person.rowguid,
                        UserName = userName
                    };
                }

                return retValue;
            }
        }
    }
}