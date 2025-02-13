using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using DataBase.Interfaces;

using MVVMLight.Messaging;

using SharedModel.DTO;
using SharedModel.Enums;

using ToDoList.Helpers;
using ToDoList.Messages;

namespace ToDoList.Pages.TaskWindow
{
    public class TaskPageViewModel
    {
        //проперти
        public NotifyProperty<string> TitlePage { get; set; } = new NotifyProperty<string>("");

        public NotifyProperty<string> Title { get; set; } = new NotifyProperty<string>("");
        public NotifyProperty<string> Body { get; set; } = new NotifyProperty<string>("");
        public NotifyProperty<DateTime> SelectedDate { get; set; } = new NotifyProperty<DateTime>(DateTime.Now);
        public ObservableCollection<StatusItem> Items { get; set; } = StatusItem.GetStatusItems();
        public NotifyProperty<StatusItem> SelectedStatus { get; set; } = new NotifyProperty<StatusItem>(null);
        public NotifyProperty<bool> PanelEnable { get; set; } = new NotifyProperty<bool>(true);

        //обработчик кнопок в зависимости от Mode
        public ICommand FirstButtonCommandAsync { get; set; }
        public NotifyProperty<string> FirstButtonText { get; set; } = new NotifyProperty<string>("");
        public NotifyProperty<Visibility> FirstButtonVisible { get; set; } = new NotifyProperty<Visibility>(Visibility.Visible);

        public ICommand SecondButtonCommandAsync { get; set; }
        public NotifyProperty<string> SecondButtonText { get; set; } = new NotifyProperty<string>("");
        public NotifyProperty<Visibility> SecondButtonVisible { get; set; } = new NotifyProperty<Visibility>(Visibility.Visible);

        public ICommand EditCommandAsync { get; set; }

        //закрытие окна
        public event Action<object> Cancel;

        private ToDoDtoModel _toDoDtoModel;
        private ToDoAction _mode;
        private IUnitOfWork<ToDoDtoModel> _dataBase;

        public TaskPageViewModel(IUnitOfWork<ToDoDtoModel> dataBaseService)
        {
            _dataBase = dataBaseService;
            FirstButtonCommandAsync = new AsyncRelayCommand(FirstButtonCommandAsyncAction);
            SecondButtonCommandAsync = new AsyncRelayCommand(SecondButtonCommandAsyncAction);

            Messenger.Default.Register<ToDoActionMessage>(this, OnModelToAction);
        }

        private async Task SecondButtonCommandAsyncAction(object? arg)
        {
            switch (_mode)
            {
                case ToDoAction.Create:
                    Cancel?.Invoke(this);
                    break;

                case ToDoAction.Edit:
                    await CancelСhange();
                    break;

                case ToDoAction.Read:
                    //тут пока ничего нет но если нужно - можно прикрутить
                    //к примеру продублировать удаление
                    break;
            }
        }

        private async Task FirstButtonCommandAsyncAction(object? arg)
        {
            switch (_mode)
            {
                case ToDoAction.Create:
                case ToDoAction.Edit:
                    await SaveToDo();
                    break;

                case ToDoAction.Read:
                    await EditToDo();
                    break;
            }
        }

        private async Task CancelСhange()
        {
            _mode = ToDoAction.Read;
            SetMode();
            SetModel();
        }

        private async Task EditToDo()
        {
            _mode = ToDoAction.Edit;
            SetMode();
        }

        private async Task SaveToDo()
        {
            await Task.Run(async () =>
            {
                try
                {
                    UpdateModel();
                    if (_mode == ToDoAction.Create)
                    {
                        _toDoDtoModel.Id = await _dataBase.CreateAsync(_toDoDtoModel);
                    }
                    else
                    {
                        await _dataBase.UpdateAsync(_toDoDtoModel);
                    }
                    Messenger.Default.Send<ToDoItemResponseMessage>(new ToDoItemResponseMessage(_toDoDtoModel));
                    Cancel?.Invoke(this);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        //Обновление данных
        private void UpdateModel()
        {
            _toDoDtoModel.UserId = Settings.USER_ID;
            _toDoDtoModel.Title = Title.Value;
            _toDoDtoModel.Description = Body.Value;
            if (_mode == ToDoAction.Create)
            {
                _toDoDtoModel.Created = DateTime.Now;
            }
            if (_mode == ToDoAction.Edit)
            {
                _toDoDtoModel.Updated = DateTime.Now;
            }
            _toDoDtoModel.DeadLine = SelectedDate.Value;
            _toDoDtoModel.Status = SelectedStatus.Value.Status;
        }

        //Установка данных
        private void SetModel()
        {
            Title.Value = _toDoDtoModel.Title;
            Body.Value = _toDoDtoModel.Description;
            if (_mode != ToDoAction.Create)
            {
                SelectedDate.Value = _toDoDtoModel.DeadLine;
            }
            SelectedStatus.Value = Items.Where(x => x.Status == _toDoDtoModel.Status).First();
        }

        //получение объекта из TaskListViewModel
        private void OnModelToAction(ToDoActionMessage message)
        {
            _toDoDtoModel = message.ResponseData;
            _mode = message.Action;
            SetMode();
        }

        //Селектор настройки View
        private void SetMode()
        {
            switch (_mode)
            {
                case ToDoAction.Create:
                case ToDoAction.Edit:
                    TitlePage.Value = _mode == ToDoAction.Create ? "Создание заметки" : "Редактирование заметки";
                    FirstButtonText.Value = "Сохранить";
                    FirstButtonVisible.Value = Visibility.Visible;
                    SecondButtonText.Value = "Отмена";
                    SecondButtonVisible.Value = Visibility.Visible;
                    PanelEnable.Value = true;
                    SetModel();
                    break;

                case ToDoAction.Read:
                    TitlePage.Value = "Просмотр заметки";
                    FirstButtonText.Value = "Изменить";
                    FirstButtonVisible.Value = Visibility.Visible;
                    SecondButtonVisible.Value = Visibility.Collapsed;
                    PanelEnable.Value = false;
                    SetModel();
                    break;
            }
        }
    }
}