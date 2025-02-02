using SharedModel.DTO;
using SharedModel.Enums;

namespace ToDoList.Messages
{
    internal class ToDoActionMessage
    {
        public ToDoDtoModel ResponseData { get; set; }

        public ToDoAction Action { get; set; }

        public ToDoActionMessage(ToDoDtoModel model, ToDoAction toDoAction)
        {
            Action = toDoAction;
            ResponseData = model;
        }
    }
}