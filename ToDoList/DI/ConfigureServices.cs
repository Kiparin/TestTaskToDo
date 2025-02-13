using Api;

using DataBase;
using DataBase.Interfaces;
using DataBase.Units;

using Microsoft.Extensions.DependencyInjection;

using SharedModel.DTO;

namespace DocumentTranslator.DependencyInjection
{
    internal class ConfigureServices
    {
        /// <summary>
        /// Инжекс сервисов приложения
        /// </summary>
        /// <param name="services"></param>
        internal static void Configure(ServiceCollection services)
        {
            services.AddTransient<ITaskApiService, TaskApiService>();
            services.AddTransient<IUnitOfWork<ToDoDtoModel>, ToDoUnit>();
        }
    }
}