using DataBase.Units;

namespace DataBase
{
    public class DataBaseService
    {
        internal const string DATABASE_NAME = "ToDo.db";
        private readonly string PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);

        public ToDoUnit ToDoTask => new ToDoUnit(PATH);
    }
}