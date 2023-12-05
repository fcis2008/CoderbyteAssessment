using DataAccessLayer.Repository.Contract;

namespace BusinessLogicLayer.Service.Service
{
    public abstract class ServiceBase
    {
        protected IRepositoryWrapper _repoWrapper;
        public ServiceBase(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
    }
}
