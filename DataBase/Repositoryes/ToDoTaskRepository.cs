using DataBase.Model;

using SQLite;

namespace DataBase.Repositoryes
{
    internal class ToDoTaskRepository : IBaseRepository<ToDoTask>
    {
        private readonly SQLiteAsyncConnection database;

        public ToDoTaskRepository(string databasePath)
        {
            database = new SQLiteAsyncConnection(databasePath);
            database.CreateTableAsync<ToDoTask>().Wait();
        }

        public async Task<int> CreateItemAsync(ToDoTask item)
        {
            return await database.InsertAsync(item);
        }

        public async Task<ToDoTask> GetItemAsync(int id)
        {
            return await database.FindAsync<ToDoTask>(id);
        }

        public async Task<IEnumerable<ToDoTask>> GetItemsAsync()
        {
            return await database.Table<ToDoTask>().ToListAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetFromQuery(string query)
        {
            return await database.QueryAsync<ToDoTask>(query);
        }

        public async Task UpdateItemAsync(ToDoTask item)
        {
            await database.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            return await database.DeleteAsync<ToDoTask>(id);
        }

        
    }
}