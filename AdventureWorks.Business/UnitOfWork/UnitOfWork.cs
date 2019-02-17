using System.Data.Entity;

namespace AdventureWorks.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext _dbContext;

        public DbContext DbContext => _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
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