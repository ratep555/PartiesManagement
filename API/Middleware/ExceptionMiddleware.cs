using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        // we want to log our exception into our terminal, display it when we start with dotnet run
        private readonly ILogger<ExceptionMiddleware> _logger;
        // we want to check in which environment we are, developing etc...
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, 
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                 var response = _env.IsDevelopment()
                    ? new ServerException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ServerException((int)HttpStatusCode.InternalServerError);
              
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response , options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}