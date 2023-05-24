using Helper.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Domain.Interfaces;
using System.Reflection;

namespace SleekFlow.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SleekFlowDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SleekFlowDbConnection"));
            });
            services.AddScoped<ISleekFlowDbContext, SleekFlowDbContext>();
            services.RegisterAllDependencies(new Type[] { typeof(IBaseRepository<>) }, Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
