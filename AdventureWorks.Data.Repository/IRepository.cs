using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AdventureWorks.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get<TEntityId>(TEntityId id) where TEntityId : struct;
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
