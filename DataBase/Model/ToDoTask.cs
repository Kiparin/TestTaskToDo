using SharedModel.Enums;

using SQLite;

namespace DataBase.Model
{
    [Table("ToDoTask")]
    internal class ToDoTask
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime Updated { get; set; }
        public Status Status { get; set; }
    }
}