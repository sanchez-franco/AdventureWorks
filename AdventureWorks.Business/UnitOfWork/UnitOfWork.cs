using System.Data.Entity;

namespace AdventureWorks.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext DbContext;

        public UnitOfWork(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}