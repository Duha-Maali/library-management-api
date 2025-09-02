using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace LibraryManagement.Presentation.Middlewares;

public class LoggingScopeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingScopeMiddleware> _logger;

    public LoggingScopeMiddleware(RequestDelegate next, ILogger<LoggingScopeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userName = context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        var roleName = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";

        Console.WriteLine($"UserName claim: {userName}, RoleName claim: {roleName}");

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["UserName"] = userName,
            ["RoleName"] = roleName
        }))
        {
            await _next(context);
        }
    }
}