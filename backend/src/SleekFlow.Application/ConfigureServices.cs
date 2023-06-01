using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Application.Features.ToDos;
using System.Reflection;

namespace SleekFlow.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Auto add the mapping profiles
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IToDoService, ToDoService>();
            return services;
        }
    }
}
