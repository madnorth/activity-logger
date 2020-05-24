using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ActivityLogger.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException e:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new
                    {
                        ValidationErrors = e.Errors.Select(f => new { f.PropertyName, f.ErrorMessage })
                    });
                    break;

                default:
                    _logger.LogError(exception, "<--! App Server Exception !-->");
                    break;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new
                {
                    Error = new
                    {
                        Message = exception.Message
                    }
                });
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}