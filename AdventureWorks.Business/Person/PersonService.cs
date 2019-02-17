using System.Collections.Generic;
using AdventureWorks.Business.UnitOfWork;
using AdventureWorks.Common;

namespace AdventureWorks.Business.Person
{
    public interface IPersonService
    {
        IList<PersonAddress> GetPersonAddresses(int personId);
    }

    public class PersonService : BaseService, IPersonService
    {
        public PersonService(IUnitOfWorkProvider unitOfWorkProvider) : base(unitOfWorkProvider)
        {
        }

        public IList<PersonAddress> GetPersonAddresses(int personId)
        {
            using (var uow = UnitOfWorkProvider.GetAdventureWorksUnitOfWork())
            {
                var retValue = new List<PersonAddress>();
                var addresses = uow.PersonRepository.GetPersonAddresses(personId);
                foreach (var address in addresses)
                {
                    retValue.Add(new PersonAddress
                    {
                        AddressType = address.AddressType.Name,
                        AddressLine1 = address.Address.AddressLine1,
                        AddressLine2 = address.Address.AddressLine2,
                        City = address.Address.City,
                        ZipCode = address.Address.PostalCode,
                        State = address.Address.StateProvince.Name
                    });
                }

                return retValue;
            }
        }
    }
}
