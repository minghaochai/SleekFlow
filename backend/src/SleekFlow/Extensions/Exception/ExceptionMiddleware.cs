using SleekFlow.Application.Common.Dtos;
using SleekFlow.Application.Common.Exceptions;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace SleekFlow.Api.Extensions.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                ErrorInfo errorInfo = error switch
                {
                    AuthenticationException e => new ErrorInfo
                    {
                        Code = StatusCodes.Status401Unauthorized,
                        Type = "common.unauthorized",
                        Message = e.Message,
                    },
                    BadRequestException e => new ErrorInfo
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Type = e.Type,
                        Message = e.Message,
                    },
                    ForbiddenException e => new ErrorInfo
                    {
                        Code = StatusCodes.Status403Forbidden,
                        Type = e.Type,
                        Message = e.Message,
                    },
                    NotFoundException e => new ErrorInfo
                    {
                        Code = StatusCodes.Status404NotFound,
                        Type = e.Type,
                        Message = e.Message,
                    },
                    OperationCanceledException => new ErrorInfo
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Type = "common.requestCancelled",
                    },
                    WebException e => new ErrorInfo
                    {
                        Code = (int)e.Status,
                        Type = "common.unknownError",
                        Message = _env.IsDevelopment() ? e.Message : "Unknown error",
                    },
                    _ => new ErrorInfo
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Type = "common.serverError",
                        Message = _env.IsDevelopment() ? error.Message : "Unknown error",
                    },
                };

                if (errorInfo.Code == StatusCodes.Status500InternalServerError || error is WebException)
                {
                    _logger.LogError(error, "{Message} - {StackTrace}", error.Message, error.StackTrace);
                }

                var errorResponse = new ErrorResponse() { Errors = new[] { errorInfo } };
                var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
                response.StatusCode = errorInfo.Code;
                await response.WriteAsync(result);
            }
        }
    }
}
