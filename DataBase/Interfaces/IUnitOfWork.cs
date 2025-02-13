namespace DataBase.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable
    {
        Task<int> CreateAsync(T item);

        Task<IEnumerable<T>> ReadAllAsync();

        Task<IEnumerable<T>> GetFromQuery(string query);

        Task UpdateAsync(T item);

        Task DeleteAsync(int id);
    }
}