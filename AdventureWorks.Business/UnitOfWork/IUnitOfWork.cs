using System;
using System.Data.Entity;

namespace AdventureWorks.Business.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        int SaveChanges();
    }
}