internal interface IBaseRepository<T>
{
    Task<int> CreateItemAsync(T item);

    Task<T> GetItemAsync(int id);

    Task<IEnumerable<T>> GetItemsAsync();

    Task UpdateItemAsync(T item);

    Task<int> DeleteItemAsync(int id);
}