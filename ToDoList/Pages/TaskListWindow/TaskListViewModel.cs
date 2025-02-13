using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

using Api;

using DataBase;
using DataBase.Helper;
using DataBase.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using MVVMLight.Messaging;

using SharedModel.DTO;
using SharedModel.Enums;
using SharedModel.Extensions;

using ToDoList.Helpers;
using ToDoList.Messages;
using ToDoList.Pages.TaskWindow;

namespace ToDoList.Pages.TaskListWindow
{
    public class TaskListViewModel
    {
        //Проперти
        public ObservableCollection<ToDoDtoModel> ToDoCollection { get; set; }

        public NotifyProperty<int> ToDoCount { get; set; } = new NotifyProperty<int>(0);
        public NotifyProperty<bool> StatusFind { get; set; } = new NotifyProperty<bool>(false);
        public NotifyProperty<bool> DateCreateFind { get; set; } = new NotifyProperty<bool>(false);
        public NotifyProperty<bool> DeadLineFind { get; set; } = new NotifyProperty<bool>(false);

        public NotifyProperty<bool> TaskPanelEnabled { get; set; } = new NotifyProperty<bool>(true);
        public NotifyProperty<bool> ButtonPanelEnabled { get; set; } = new NotifyProperty<bool>(true);
        public NotifyProperty<Visibility> FindPanelVisibleEnabled { get; set; } = new NotifyProperty<Visibility>(Visibility.Hidden);

        public NotifyProperty<DateTime> DateCreate { get; set; } = new NotifyProperty<DateTime>(DateTime.Now);
        public NotifyProperty<DateTime> DeadLine { get; set; } = new NotifyProperty<DateTime>(DateTime.Now);
        public ObservableCollection<StatusItem> Status { get; set; } = StatusItem.GetStatusItems();
        public NotifyProperty<StatusItem> SelectedStatus { get; set; } = new NotifyProperty<StatusItem>(StatusItem.GetStatusItems().First());

        //Комманды
        public ICommand CreateCommandAsync { get; set; }

        public ICommand ReadToDoCommandAsync { get; set; }
        public ICommand DeleteToDoCommandAsync { get; set; }
        public ICommand SynchToDoCommandAsync { get; set; }
        public ICommand PrintCommandAsync { get; set; }
        public ICommand OpenFindCommandAsync { get; set; }
        public ICommand CancelFindCommandAsync { get; set; }
        public ICommand FindCommandAsync { get; set; }

        //Инджекшены
        private IUnitOfWork<ToDoDtoModel> _dataBaseService;

        private ITaskApiService _apiService;

        public TaskListViewModel(IUnitOfWork<ToDoDtoModel> dataBaseService, ITaskApiService apiService)
        {
            _dataBaseService = dataBaseService;
            _apiService = apiService;

            ToDoCollection = new ObservableCollection<ToDoDtoModel>();
            ToDoCount.Value = ToDoCollection.Count;

            //Подписываем комманды
            CreateCommandAsync = new AsyncRelayCommand(CreateNewToDoAsync);
            ReadToDoCommandAsync = new AsyncRelayCommand(ReadToDoAsync);
            DeleteToDoCommandAsync = new AsyncRelayCommand(DeleteToDoAsync);
            SynchToDoCommandAsync = new AsyncRelayCommand(SynchToDoAsync);
            PrintCommandAsync = new AsyncRelayCommand(PrintToDoAsync);

            //Команды панели
            OpenFindCommandAsync = new AsyncRelayCommand(OpenFindAsync);
            CancelFindCommandAsync = new AsyncRelayCommand(CancelFindAsync);
            FindCommandAsync = new AsyncRelayCommand(FindAsync);

            //Подписываемся на месседжер
            Messenger.Default.Register<ToDoItemResponseMessage>(this, OnToDoItemReceived);
            Task.Run(async () => { await GetToDoFomDataBase(); });
        }

        private async Task FindAsync(object? arg)
        {
            if (StatusFind.Value || DateCreateFind.Value || DeadLineFind.Value)
            {
                var query = QueryHelper.MakeSelect
                    (
                      StatusFind.Value ? (int)SelectedStatus.Value.Status : null,
                      DateCreateFind.Value ? DateCreate.Value : null,
                      DeadLineFind.Value ? DeadLine.Value : null
                    );
                var result = await _dataBaseService.GetFromQuery(query);
                ToDoCollection.UpdateFromList(result);
                await UpdateCount();
            }
            else
            {
                await GetToDoFomDataBase();
            }
        }

        private async Task CancelFindAsync(object? arg)
        {
            TaskPanelEnabled.Value = true;
            ButtonPanelEnabled.Value = true;
            FindPanelVisibleEnabled.Value = Visibility.Hidden;

            if (!StatusFind.Value || !DateCreateFind.Value || !DeadLineFind.Value)
            {
                await GetToDoFomDataBase();
            }
        }

        private async Task OpenFindAsync(object? arg)
        {
            TaskPanelEnabled.Value = false;
            ButtonPanelEnabled.Value = false;
            FindPanelVisibleEnabled.Value = Visibility.Visible;
        }

        private async Task PrintToDoAsync(object? arg)
        {
            PrintDialog printDialog = new PrintDialog();
            var pringText = PrintHelper.MakeTextFrom(ToDoCollection);
            printDialog.PrintDocument(((IDocumentPaginatorSource)pringText).DocumentPaginator, "Задача на печать");
        }

        //Синхронизация
        private async Task SynchToDoAsync(object? arg)
        {
            var collection = await _apiService.GetAllAsync(Settings.USER_ID);
            var missingElements = collection.Where(
                item => !ToDoCollection.Any(o => o.Id == item.Id
                )).ToList();

            if (missingElements.Any())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ToDoCollection.AddRange(missingElements);
                });

                foreach (var item in missingElements)
                {
                    await _dataBaseService.CreateAsync(item);
                    Console.WriteLine(item.Description);
                }

                await UpdateCount();
            }
        }

        //Удалить
        private async Task DeleteToDoAsync(object? arg)
        {
            if (arg != null && arg is ToDoDtoModel todoItem)
            {
                ToDoCollection.Remove(todoItem);
                await _dataBaseService.DeleteAsync(todoItem.Id);
                await UpdateCount();
            }
        }

        //Открыть
        private async Task ReadToDoAsync(object? arg)
        {
            if (arg != null && arg is ToDoDtoModel todoItem)
            {
                await OpenPage(todoItem, ToDoAction.Read);
            }
        }

        //Создать
        private async Task CreateNewToDoAsync(object? arg)
        {
            await OpenPage(new ToDoDtoModel(), ToDoAction.Create);
        }

        //Открытие страницы
        private async Task OpenPage(ToDoDtoModel model, ToDoAction action)
        {
            var taskPage = App.ServiceProvider.GetService<TaskPage>();
            Messenger.Default.Send(new ToDoActionMessage(model, action));

            taskPage?.ShowDialog();
        }

        //При добавлении/обновлении новый ToDoDtoModel упадет сюда
        private async void OnToDoItemReceived(ToDoItemResponseMessage message)
        {
            //Проверка, существует ли элемент с таким же Id
            var existingItem = ToDoCollection.FirstOrDefault(item => item.Id == message.ResponseData.Id);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (existingItem != null)
                {
                    //Если элемент существует, заменяем его
                    var index = ToDoCollection.ToList().FindIndex(item => item.Id == message.ResponseData.Id);
                    ToDoCollection.RemoveAt(index); // Удаляем старый элемент
                    ToDoCollection.Insert(index, message.ResponseData); // Вставляем обновленный элемент
                }
                else
                {
                    //Если элемент не существует, добавляем его в коллекцию
                    ToDoCollection.Add(message.ResponseData);
                }
            });

            await UpdateCount();
        }

        //Получение всех задач из базы  ( в нашем случае только получение тасок )
        private async Task GetToDoFomDataBase()
        {
            var result = await _dataBaseService.ReadAllAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                ToDoCollection.UpdateFromList(result);
            });
            await UpdateCount();
        }

        //обновляем счетчик задач
        private async Task UpdateCount()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ToDoCount.Value = ToDoCollection.Count();
            });
        }
    }
}