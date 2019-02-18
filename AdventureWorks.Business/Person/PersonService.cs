using System.Collections.Generic;
using System.Linq;
using AdventureWorks.Business.UnitOfWork;
using AdventureWorks.Common;

namespace AdventureWorks.Business.Person
{
    public interface IPersonService
    {
        PersonDetail GetPerson(int personId);
        IList<PersonAddress> GetPersonAddresses(int personId);
    }

    public class PersonService : BaseService, IPersonService
    {
        public PersonService(IUnitOfWorkProvider unitOfWorkProvider) : base(unitOfWorkProvider)
        {
        }

        public PersonDetail GetPerson(int personId)
        {
            using (var uow = UnitOfWorkProvider.GetAdventureWorksUnitOfWork())
            {
                PersonDetail retValue = null;
                var entity = uow.PersonRepository.GetPersonDetail(personId);
                if (entity != null)
                {
                    var address = entity.BusinessEntityAddresses.FirstOrDefault();
                    retValue = new PersonDetail
                    {
                        AddressType = address?.AddressType.Name,
                        AddressLine1 = address?.Address.AddressLine1,
                        AddressLine2 = address?.Address.AddressLine2,
                        City = address?.Address.City,
                        ZipCode = address?.Address.PostalCode,
                        State = address?.Address.StateProvince.Name,
                        FirstName = entity?.Person.FirstName,
                        LastName = entity?.Person.LastName
                    };
                }

                return retValue;
            }
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
