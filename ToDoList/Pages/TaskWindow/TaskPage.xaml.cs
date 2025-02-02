using System.Windows;

namespace ToDoList.Pages.TaskWindow
{
    /// <summary>
    /// Interaction logic for TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Window
    {
        public TaskPage(TaskPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.Cancel += cloce => CancelWindow(cloce);
        }

        private void CancelWindow(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Close();
            });
        }
    }
}