using Microsoft.Extensions.DependencyInjection;

namespace DocumentTranslator.DependencyInjection
{
    internal class ConfigureDependencyInjection
    {
        /// <summary>
        /// Настройка DI контейнеров
        /// </summary>
        /// <param name="services"></param>
        internal static void Configure(ServiceCollection services)
        {
            ConfigurePage.Configure(services);
            ConfigureViewModels.Configure(services);
            ConfigureServices.Configure(services);
        }
    }
}