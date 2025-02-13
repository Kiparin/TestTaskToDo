namespace DataBase
{
    public abstract class IBaseDataService
    {
        private readonly string DATABASE_NAME = "ToDo.db";
        internal readonly string dataPath;

        protected IBaseDataService()
        {
            dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);
        }
    }
}