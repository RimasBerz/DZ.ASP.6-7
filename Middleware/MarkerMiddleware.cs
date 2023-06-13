using WebApplication1.Data;

namespace WebApplication1.Middleware
{
    public class MarkerMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _getCount;
        private static int _postCount;

        public MarkerMiddleware(RequestDelegate next)
        {
            _next = next;
            _getCount = 0;
            _postCount = 0;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            if (context.Request.Method == HttpMethods.Get)
            {
                _getCount++;
            }
            else if (context.Request.Method == HttpMethods.Post)
            {
                _postCount++;
            }

            context.Items.Add("marker", $"{dataContext.Users.Count()} users, GET: {_getCount}, POST: {_postCount}");

            await _next(context);
        }
    }

    public static class MarkerMiddlewareExtension
    {
        public static IApplicationBuilder UseMarker(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MarkerMiddleware>();
        }
    }
}
