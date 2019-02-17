namespace AdventureWorks.Business.UnitOfWork
{
    public interface IUnitOfWorkProvider
    {
        IAdventureWorksUnitOfWork GetAdventureWorksUnitOfWork();
    }
}