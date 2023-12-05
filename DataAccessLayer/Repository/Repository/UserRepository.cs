using DataAccessLayer.Model;
using DataAccessLayer.Repository.Contract;

namespace DataAccessLayer.Repository.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(CoderByteAssessmentDbContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
