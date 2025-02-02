namespace DataBase.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable
    {
        Task<int> CreateAsync(T item);

        Task<IEnumerable<T>> ReadAllAsync();

        Task UpdateAsync(T item);

        Task DeleteAsync(int id);
    }
}