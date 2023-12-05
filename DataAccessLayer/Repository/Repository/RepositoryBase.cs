using DataAccessLayer.Model;
using DataAccessLayer.Repository.Contract;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected CoderByteAssessmentDbContext CoderByteAssessmentDbContext { get; set; }

        public RepositoryBase(CoderByteAssessmentDbContext coderByteAssessmentDbContext)
        {
            CoderByteAssessmentDbContext = coderByteAssessmentDbContext;
        }

        public async Task<T> FindByIdAsync(string id)
        {
            return await CoderByteAssessmentDbContext.Set<T>().FindAsync(id);
        }

        public void Create(T entity)
        {
            CoderByteAssessmentDbContext.Set<T>().Add(entity);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return CoderByteAssessmentDbContext.Set<T>().Where(expression);
        }
    }
}
