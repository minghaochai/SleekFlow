namespace SleekFlow.Api.Extensions.Cors
{
    public static class CorsExtension
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptions = new CorsOptions();
            configuration.GetSection(CorsOptions.Cors).Bind(corsOptions);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.WithOrigins(corsOptions.GetOrigins())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            return services;
        }
    }
}
