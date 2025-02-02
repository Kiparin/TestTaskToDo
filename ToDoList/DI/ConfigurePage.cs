using Microsoft.Extensions.DependencyInjection;

using ToDoList.Pages.TaskListWindow;
using ToDoList.Pages.TaskWindow;

namespace DocumentTranslator.DependencyInjection
{
    internal class ConfigurePage
    {
        /// <summary>
        /// Инжект страниц приложения
        /// </summary>
        /// <param name="services"></param>
        internal static void Configure(ServiceCollection services)
        {
            services.AddTransient<TaskListPage>();
            services.AddTransient<TaskPage>();
        }
    }
}