using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Contract
{
    public interface IRepositoryBase<T>
    {
        Task<T> FindByIdAsync(string id);
        void Create(T entity);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        //More methods can be added below
    }
}
