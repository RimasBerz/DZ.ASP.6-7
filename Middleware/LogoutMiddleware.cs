using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class LogoutMiddleware
{
    private readonly RequestDelegate _next;

    public LogoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/User/Logout" && context.Request.Method == "POST")
        {
            context.Session.Clear(); 

        }
        await _next(context);
    }
}