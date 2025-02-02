using System.Text;
using System.Threading.Tasks;

namespace DataBase.Helper
{
    public static class QueryHelper
    {
        //Лобовое решение
        public static string MakeSelect(int? status = null, DateTime? createData = null, DateTime? deadLine = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Select * from ToDoTask Where ");
            if (status != null)
            {
                stringBuilder.AppendLine($"Status = {status} AND ");
            }
            if (createData != null)
            {
                stringBuilder.AppendLine($"Created = {((DateTime)createData).Ticks} AND ");
            }
            if (deadLine != null)
            {
                stringBuilder.AppendLine($"DeadLine = {((DateTime)deadLine).Ticks} AND ");
            }
            var result = stringBuilder.ToString().Replace("\n", "").Replace("\r", "");

            return result.Substring(0, result.Length - 4);
        }
    }
}
