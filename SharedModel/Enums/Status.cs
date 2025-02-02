using System.Collections.ObjectModel;
using System.ComponentModel;

using SharedModel.Enums;
using SharedModel.Extensions;

namespace SharedModel.Enums
{
    public enum Status
    {
        [Description("В процессе")]
        InProcess,

        [Description("Успешно")]
        Success,

        [Description("Отменено")]
        Cancelled
    }
}

public class StatusItem
{
    public int Id { get; set; }
    public Status Status { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Получение всех статусов ToDo
    /// </summary>
    /// <returns></returns>
    public static ObservableCollection<StatusItem> GetStatusItems()
    {
        var items = new ObservableCollection<StatusItem>();

        foreach (Status status in Enum.GetValues(typeof(Status)))
        {
            items.Add(new StatusItem()
            {
                Id = (int)status,
                Status = status,
                Name = status.GetDescription()
            });
        }

        return items;
    }
}