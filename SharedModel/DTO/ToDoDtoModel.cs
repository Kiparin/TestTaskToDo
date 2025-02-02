using SharedModel.Enums;
using SharedModel.Extensions;

namespace SharedModel.DTO
{
    public class ToDoDtoModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime Updated { get; set; }
        public Status Status { get; set; }

        public string StatusDescription
        {
            get
            {
                return Status.GetDescription();
            }
        }
    }
}