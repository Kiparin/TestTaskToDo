using System.Globalization;
using System.Windows.Data;

using SharedModel.DTO;

namespace ToDoList.Converters
{
    public class DateSelectorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ToDoDtoModel data)
            {
                // Логика выбора между датой создания и датой изменения
                var selectedDate = data.Created > data.Updated ? data.Created : data.Updated;
                return selectedDate.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}