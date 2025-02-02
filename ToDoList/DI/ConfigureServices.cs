using Api;

using DataBase;

using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<TaskApiService>();
            services.AddTransient<DataBaseService>();
        }
    }
}