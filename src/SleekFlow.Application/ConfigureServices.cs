using Helper.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Application.Features.Base;
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

            services.RegisterAllDependencies(new Type[] { typeof(IBaseService<>) }, Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
