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
                long startTicks = ((DateTime)createData).Date.Ticks; // Начало дня
                long endTicks = ((DateTime)createData).Date.AddDays(1).Ticks - 1; // Конец дня
                stringBuilder.AppendLine($"Created BETWEEN {startTicks} AND {endTicks} AND ");
            }
            if (deadLine != null)
            {
                // Приводим даты к 00:00
                long startTicks = ((DateTime)deadLine).Date.Ticks; // Начало дня
                long endTicks = ((DateTime)deadLine).Date.AddDays(1).Ticks - 1; // Конец дня
                stringBuilder.AppendLine($"DeadLine BETWEEN {startTicks} AND {endTicks} AND ");
            }
            var result = stringBuilder.ToString().Replace("\n", "").Replace("\r", "");

            return result.Substring(0, result.Length - 4);
        }
    }
}
