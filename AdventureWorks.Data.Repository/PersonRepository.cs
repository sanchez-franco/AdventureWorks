using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AdventureWorks.Data.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        BusinessEntity GetPersonDetail(int personId);
        Person GetPerson(string emailAddress, string passwordSalt);
        IList<BusinessEntityAddress> GetPersonAddresses(int personId);
    }

    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(DbContext dbContext) : base(dbContext)
        {
        }

        //Let's assume that we "encrypt" to match the password in the DB
        public Person GetPerson(string emailAddress, string passwordSalt)
        {
            return DbContext.Set<EmailAddress>()
                .Include(r => r.Person)
                .FirstOrDefault(r => r.EmailAddress1 == emailAddress && r.Person.Password.PasswordSalt == passwordSalt)
                ?.Person;
        }

        public BusinessEntity GetPersonDetail(int personId)
        {
            return DbContext.Set<BusinessEntity>()
                .Include(r => r.Person)
                .Include(r => r.BusinessEntityAddresses)
                .Include(r => r.BusinessEntityAddresses.Select(x => x.Address))
                .Include(r => r.BusinessEntityAddresses.Select(x => x.Address.StateProvince))
                .Include(r => r.BusinessEntityAddresses.Select(x => x.AddressType))
                .FirstOrDefault(r => r.BusinessEntityID == personId);
        }

        public IList<BusinessEntityAddress> GetPersonAddresses(int personId)
        {
            return DbContext.Set<BusinessEntityAddress>()
                .Include(r => r.Address)
                .Include(r => r.Address.StateProvince)
                .Include(r => r.AddressType)
                .Where(r => r.BusinessEntityID == personId)
                .ToList();
        }
    }
}