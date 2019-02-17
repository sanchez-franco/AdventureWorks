using AdventureWorks.Data;
using AdventureWorks.Data.Repository;

namespace AdventureWorks.Business.UnitOfWork
{
    public class AdventureWorksUnitOfWork : UnitOfWork, IAdventureWorksUnitOfWork
    {
        public AdventureWorksUnitOfWork(AdventureWorks2014Entities dbContext) : base(dbContext)
        {
        }

        #region Private Variables

        private IPersonRepository _personRepository;

        #endregion

        #region Public Properties

        public IPersonRepository PersonRepository => _personRepository ?? (_personRepository = new PersonRepository(DbContext));

        #endregion
    }
}