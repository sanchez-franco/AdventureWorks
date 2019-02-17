using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AdventureWorks.Data.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
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

        public IList<BusinessEntityAddress> GetPersonAddresses(int personId)
        {
            return DbContext.Set<BusinessEntityAddress>()
                .Include(r => r.Address)
                .Include(r => r.AddressType)
                .Include(r => r.Address.StateProvince)
                .Where(r => r.BusinessEntityID == personId)
                .ToList();
        }
    }
}