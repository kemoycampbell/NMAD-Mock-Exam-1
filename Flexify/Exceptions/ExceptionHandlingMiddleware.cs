using System;
using System.Threading.Tasks;
using Flexify.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Flexify.Services
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.Info(context.Request.Method + "- "+ context.Request.Path);
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //default message
            int statusCode = 500;
            string message = "An internal error occurred";
            
            //if the error is instance of user exception, we want to show that message and status to the user
            if (exception is IUserErrorException)
            {
                UserException err = (UserException) exception;
                statusCode = err.GetStatusCode();
                message = err.GetMessage();
            }
            
            //logging internal errors
            if (statusCode == 500)
            {
                _logger.Error(exception.Message);
            }

            //we want the response to output as json
            context.Response.ContentType = "application/json";
            string errorString = JsonSerializer.Serialize(new {Message = message, StatusCode = statusCode});
            await context.Response.WriteAsync(errorString);
        }
    }
}