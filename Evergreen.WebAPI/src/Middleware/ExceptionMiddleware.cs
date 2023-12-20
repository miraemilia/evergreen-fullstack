using Evergreen.Service.src.Shared;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DbUpdateException e)
        {
            await context.Response.WriteAsync(e.InnerException.Message);
        }
        catch (CustomException e)
        {
            context.Response.StatusCode = e.StatusCode;
            await context.Response.WriteAsJsonAsync(e.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(e.Message);
        }
    }
}