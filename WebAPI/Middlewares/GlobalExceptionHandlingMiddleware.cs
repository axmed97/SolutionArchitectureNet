using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)ex.StatusCode;

                ProblemDetails problem = new()
                {
                    Status = (int)ex.StatusCode,
                    Type = ex.HttpRequestError.ToString(),
                    Title = ex.InnerException.Message,
                    Detail = ex.Message
                };
                var json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = ex.Message
                };
                var json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }
    }
}
