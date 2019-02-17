using AdventureWorks.Business.UnitOfWork;

namespace AdventureWorks.Business
{
    public class BaseService
    {
        protected readonly IUnitOfWorkProvider UnitOfWorkProvider;

        public BaseService(IUnitOfWorkProvider unitOfWorkProvider)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }
}
