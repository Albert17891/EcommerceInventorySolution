using EcommerceInventory.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Api.Middlewares;

public class AuthMiddleware
{
    private RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AppDbContext dbContext)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader["Bearer ".Length..].Trim();
            var session = await dbContext.Sessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);

            if (session != null)
            {
                context.Items["User"] = session.User;
            }
        }

        await _next(context);
    }
}
