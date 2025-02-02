using Api.Models.External;

using SharedModel.DTO;
using SharedModel.Enums;

namespace Api
{
    //TOOD Переделать на AutoMapper
    /// <summary>
    /// Маппер объектов API
    /// </summary>
    internal static class Mapper
    {
        internal static List<ToDoDtoModel> MapToInternalList(List<ToDoModel> externalModel)
        {
            var result = new List<ToDoDtoModel>();

            foreach (var item in externalModel)
            {
                var toDoItem = new ToDoDtoModel
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    Title = item.Title,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Description = item.Title
                };
                if (item.Completed)
                {
                    toDoItem.DeadLine = DateTime.Now;
                    toDoItem.Status = Status.Success;
                }
                else
                {
                    toDoItem.DeadLine = DateTime.Now.AddDays(3);
                    toDoItem.Status = Status.InProcess;
                }

                result.Add(toDoItem);
            }

            return result;
        }
    }
}