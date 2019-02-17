using AdventureWorks.Data;

namespace AdventureWorks.Business.UnitOfWork
{
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        public IAdventureWorksUnitOfWork GetAdventureWorksUnitOfWork()
        {
            var dbContext = new AdventureWorks2014Entities
            {
                Database =
                {
                    CommandTimeout = TransactionHelper.Timeout
                }
            };

            return new AdventureWorksUnitOfWork(dbContext);
        }
    }
}