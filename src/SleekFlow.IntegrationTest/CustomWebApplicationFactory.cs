using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SleekFlow.IntegrationTest
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        // https://github.com/dotnet/aspnetcore/issues/37680 details the problem with .net6 overriding configuration from the web project
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string> { { "ConnectionStrings:SleekFlowDbConnection", "Server=(localdb)\\MSSQLLocalDB;Database=SleekFlowTest;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;" } });
            });

            return base.CreateHost(builder);
        }
    }
}
