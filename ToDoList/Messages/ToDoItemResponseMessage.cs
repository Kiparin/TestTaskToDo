using SharedModel.DTO;

namespace ToDoList.Messages
{
    internal class ToDoItemResponseMessage
    {
        public ToDoDtoModel ResponseData { get; set; }

        public ToDoItemResponseMessage(ToDoDtoModel model)
        {
            ResponseData = model;
        }
    }
}