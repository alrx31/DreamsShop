using System.Net;
using System.Text.Json;
using Application.Exceptions;

namespace Presentation.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next1)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context,HttpStatusCode.NotFound,ex.Message);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorDetails = new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
        };

        return context.Response.WriteAsync(errorDetails.ToString());
    }
    
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public required string  Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}