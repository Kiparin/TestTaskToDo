using Api.Models.External;

using Refit;

namespace Api.Interfaces
{
    internal interface IUsersToDoApi
    {
        [Get("/users/{userId}/todos")]
        Task<List<ToDoModel>> GetAll(int userId);
    }
}