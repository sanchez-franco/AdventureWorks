using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace AdventureWorks.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext DbContext;

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TEntity Get<TEntityId>(TEntityId id) where TEntityId : struct
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate).ToList();
        }

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
        }
    }
}