namespace DataAccessLayer.Repository.Contract
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        T MapObjects<T>(T source, T target) where T : class, new();
        Task Save();
    }
}
