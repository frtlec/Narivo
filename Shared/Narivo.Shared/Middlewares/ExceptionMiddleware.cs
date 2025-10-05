using Microsoft.AspNetCore.Http;
using Narivo.Shared.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Narivo.Shared.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            string errorCode;
            string message;
            object? details = null;

            switch (ex)
            {
                case AppException appEx:
                    statusCode = appEx.StatusCode;
                    errorCode = appEx.ErrorCode;
                    message = appEx.Message;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorCode = "UNEXPECTED_ERROR";
                    message = "Beklenmeyen bir hata oluştu.";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                success = false,
                errorCode,
                message,
                details
            });

            await context.Response.WriteAsync(result);
        }
    }
}
