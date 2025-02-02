using System.Windows.Input;

namespace ToDoList.Helpers
{
    /// <summary>
    /// Асинхронный биндинг для Command MVVM
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object?, Task> _execute;
        private readonly Predicate<object?>? _canExecute;
        private ICommand? openFindCommandAsync;

        public AsyncRelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public AsyncRelayCommand(ICommand? openFindCommandAsync)
        {
            this.openFindCommandAsync = openFindCommandAsync;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public async void Execute(object? parameter)
        {
            await _execute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}