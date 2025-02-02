using System.Windows;

using DocumentTranslator.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using ToDoList.Pages.TaskListWindow;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        //Точка доступа в контейнер DependencyInjection
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureDependencyInjection.Configure(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            ServiceProvider = _serviceProvider;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetService<TaskListPage>();
            mainWindow.Show();
        }
    }
}