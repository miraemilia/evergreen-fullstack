using Evergreen.Service.src.Shared;

namespace Evergreen.WebAPI.src.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (CustomException e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = e.StatusCode;
            await context.Response.WriteAsJsonAsync(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(e.Message);
        }
    }
}