using Microsoft.Extensions.DependencyInjection;

using ToDoList.Pages.TaskListWindow;
using ToDoList.Pages.TaskWindow;

namespace DocumentTranslator.DependencyInjection
{
    internal class ConfigureViewModels
    {
        /// <summary>
        /// Инжект ViewModel приложения
        /// </summary>
        /// <param name="services"></param>
        internal static void Configure(ServiceCollection services)
        {
            services.AddTransient<TaskListViewModel>();
            services.AddTransient<TaskPageViewModel>();
        }
    }
}