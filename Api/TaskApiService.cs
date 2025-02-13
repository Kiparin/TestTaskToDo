using Api.Interfaces;

using Refit;

using SharedModel.DTO;

namespace Api
{
    public class TaskApiService : ITaskApiService 
    {
        private readonly IUsersToDoApi _userApi;

        public TaskApiService()
        {
            _userApi = RestService.For<IUsersToDoApi>(ConnectionString.Connection);
        }

        /// <summary>
        /// Получить все записи заметок по выделенному пользователю.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ToDoDtoModel>> GetAllAsync(int userId)
        {
            try
            {
                var result = await _userApi.GetAll(userId);
                return Mapper.MapToInternalList(result);
            }
            // тут надо сегментировать ошибоки от редкой до общей
            catch (Exception e)
            {
                //Пишем лог сбоя в консоль
                //TODO Сделать Logger
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}