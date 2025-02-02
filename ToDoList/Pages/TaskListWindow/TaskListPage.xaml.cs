using System.Windows;

namespace ToDoList.Pages.TaskListWindow
{
    /// <summary>
    /// TaskListPage.xaml
    /// </summary>
    public partial class TaskListPage : Window
    {
        public TaskListPage(TaskListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}