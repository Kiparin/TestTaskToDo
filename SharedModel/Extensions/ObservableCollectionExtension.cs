using System.Collections.ObjectModel;

namespace SharedModel.Extensions
{
    /// <summary>
    /// Работа с коллекцией ObservableCollectionExtension
    /// </summary>
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// Обновление коллекции из другой коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observableCollection"></param>
        /// <param name="newList"></param>
        public static void UpdateFromList<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> newList)
        {
            observableCollection.Clear();

            foreach (var item in newList)
            {
                observableCollection.Add(item);
            }
        }

        /// <summary>
        /// Добавление коллекции из другой коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observableCollection"></param>
        /// <param name="newList"></param>
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> newList)
        {
            foreach (var item in newList)
            {
                observableCollection.Add(item);
            }
        }
    }
}