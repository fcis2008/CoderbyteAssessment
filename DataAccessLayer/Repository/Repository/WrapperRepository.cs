using DataAccessLayer.Model;
using DataAccessLayer.Repository.Contract;
using System.Reflection;

namespace DataAccessLayer.Repository.Repository
{
    public class WrapperRepository : IRepositoryWrapper
    {
        private readonly CoderByteAssessmentDbContext _repoContext;

        private IUserRepository _user;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }


        public WrapperRepository(CoderByteAssessmentDbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public T MapObjects<T>(T source, T target) where T : class, new()
        {
            var properties = typeof(T).GetProperties();
            foreach (PropertyInfo sourceProp in properties)
            {
                PropertyInfo targetProp = properties.Where(p => p.Name == sourceProp.Name).FirstOrDefault();
                if (targetProp != null && targetProp.GetType().Name == sourceProp.GetType().Name)
                {
                    targetProp.SetValue(target, sourceProp.GetValue(source));
                }
            }
            return target;
        }

        public async Task Save()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
