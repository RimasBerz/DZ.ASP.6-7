using System.Security.Claims;
using WebApplication1.Data;

namespace WebApplication1.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            if (context.Session.Keys.Contains("userId"))
            {
                var user = dataContext.Users.Find(Guid.Parse(
                    context.Session.GetString("userId")!
                    ));
                if(user != null)
                {
                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Login),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.UserData, user.Avatar ??  "")
                    };
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, nameof(AuthSessionMiddleware)
                        ));
                }
            }
            await _next(context);
        }
    }
    public static class AuthSessionMiddlewareExtension
    {
        public static IApplicationBuilder
          UseAuthSession(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthSessionMiddleware>();
        }

    }
}
