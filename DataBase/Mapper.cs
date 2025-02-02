using DataBase.Model;

using SharedModel.DTO;

namespace DataBase
{
    internal static class Mapper
    {
        internal static ToDoDtoModel ToDto(this ToDoTask task)
        {
            if (task == null)
                return null;

            return new ToDoDtoModel
            {
                Id = task.Id,
                UserId = task.UserId,
                Title = task.Title,
                Description = task.Description,
                Created = task.Created,
                DeadLine = task.DeadLine,
                Updated = task.Updated,
                Status = task.Status
            };
        }

        internal static ToDoTask ToModel(this ToDoDtoModel dto)
        {
            if (dto == null)
                return null;

            return new ToDoTask
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                Created = dto.Created,
                DeadLine = dto.DeadLine,
                Updated = dto.Updated,
                Status = dto.Status
            };
        }

        internal static IEnumerable<ToDoDtoModel> ToDtoList(this IEnumerable<ToDoTask> tasks)
        {
            return tasks.Select(task => task.ToDto()).ToList(); // Применяем ToDto ко всем элементам
        }
    }
}