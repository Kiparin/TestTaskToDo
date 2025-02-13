
using SharedModel.DTO;

namespace Api
{
    public interface ITaskApiService
    {
        Task<List<ToDoDtoModel>> GetAllAsync(int userId);
    }
}