namespace SleekFlow.Api.Extensions.Exception
{
    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
