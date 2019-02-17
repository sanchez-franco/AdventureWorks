using AdventureWorks.Data.Repository;

namespace AdventureWorks.Business.UnitOfWork
{
    public interface IAdventureWorksUnitOfWork : IUnitOfWork
    {
        IPersonRepository PersonRepository { get; }
    }
}