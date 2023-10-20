using ExercicioBnp.Exceptions;
using System.Net;

namespace ExercicioBnp.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext httpContext, Exception ex)
        {
            if (ex is CustomException customEx)
            {
                httpContext.Response.StatusCode = (int)customEx.StatusCode;
                await httpContext.Response.WriteAsync(customEx.Message);
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync($"An unexpected error occurred. ");
            }
        }
    }
}
