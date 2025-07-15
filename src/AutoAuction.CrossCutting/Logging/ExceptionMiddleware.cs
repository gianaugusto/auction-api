using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AutoAuction.CrossCutting.Logging
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Set appropriate status code based on exception type
            int statusCode;
            string errorType;

            switch (exception)
            {
                case ArgumentException argEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorType = "ValidationError";
                    break;
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errorType = "NotFound";
                    break;
                case InvalidOperationException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    errorType = "Conflict";
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorType = "ServerError";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new
            {
                errorType = errorType,
                message = exception.Message,
                details = exception.InnerException?.Message
            });

            return context.Response.WriteAsync(result);
        }
    }
}
