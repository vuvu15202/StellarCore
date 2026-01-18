using Microsoft.AspNetCore.Http;
using Stellar.Shared.Models.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Stellar.Shared.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new ErrorResponse
            {
                Timestamp = DateTime.Now,
                Path = context.Request.Path,
                TraceId = context.TraceIdentifier
            };

            // Can extend this with specific exception types like ValidationException, etc.
            // For now, generic handler.

            if (exception is ResponseException responseException)
            {
                context.Response.StatusCode = responseException.StatusCode;
                response.Status = responseException.StatusCode;
                response.MessageCode = responseException.MessageCode;
                response.Message = responseException.Message;
            }
            else if (exception is ArgumentException || exception is InvalidOperationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = (int)HttpStatusCode.BadRequest;
                response.MessageCode = CommonErrorMessage.VALIDATION_FAILED;
                response.Message = exception.Message;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.MessageCode = CommonErrorMessage.INTERNAL_SERVER;
                response.Message = "An internal server error occurred.";
                // In dev, maybe show exception message
                // response.Message = exception.Message;
            }

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
