using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Documents;

using SharedModel.DTO;

namespace ToDoList.Helpers
{
    public static class PrintHelper
    {
        public static FlowDocument MakeTextFrom(ObservableCollection<ToDoDtoModel> collection)
        {
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(MakeParagraphMaket(collection));
            return flowDocument;
        }

        private static Paragraph MakeParagraphMaket(ObservableCollection<ToDoDtoModel> collection)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var task in collection)
            {
                stringBuilder.AppendLine($"Задача #{task.Id}: {task.Title}");
                stringBuilder.AppendLine($"Описание: {task.Description}");
                stringBuilder.AppendLine($"Дата создания: {task.Created.ToString("dd.MM.yyyy")}");
                stringBuilder.AppendLine($"Дедлайн: {task.DeadLine.ToString("dd.MM.yyyy")}");
                stringBuilder.AppendLine($"Статус: {task.StatusDescription}");
                stringBuilder.AppendLine();
            }

            return new Paragraph(new Run(stringBuilder.ToString()))
            {
                Margin = new Thickness(0, 0, 0, 10)
            };
        }
    }
}