using DataBase.Interfaces;
using DataBase.Repositoryes;

using SharedModel.DTO;

namespace DataBase.Units
{
    /// <summary>
    /// Класс-прослойка между репозиторием и приложением
    /// Принимает модели приложения конвертирует в свой слой
    /// Оверинжиниринг с прицелом на расширение
    /// </summary>
    public class ToDoUnit : IUnitOfWork<ToDoDtoModel>
    {
        private ToDoTaskRepository _dataBase;

        public ToDoUnit(string dataBasePath)
        {
            _dataBase = new ToDoTaskRepository(dataBasePath);
        }

        /// <summary>
        /// Создает объекта в базе данных
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ToDoDtoModel item)
        {
            var model = Mapper.ToModel(item);
            return await _dataBase.CreateItemAsync(model);
        }

        /// <summary>
        /// Вернуть все объекты из базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ToDoDtoModel>> ReadAllAsync()
        {
            var items = await _dataBase.GetItemsAsync();
            return items.ToDtoList();
        }

        public async Task<IEnumerable<ToDoDtoModel>> GetFromQuery(string query)
        {
            var items = await _dataBase.GetFromQuery(query);
            return items.ToDtoList();
        }

        /// <summary>
        /// Обновление объекта базы данных
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ToDoDtoModel item)
        {
            await _dataBase.UpdateItemAsync(item.ToModel());
        }

        /// <summary>
        /// Удаление объекта из базы данных
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            await _dataBase.DeleteItemAsync(id);
        }

        public void Dispose()
        {
        }
    }
}