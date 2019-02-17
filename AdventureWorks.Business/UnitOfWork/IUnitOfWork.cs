using System;

namespace AdventureWorks.Business.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}