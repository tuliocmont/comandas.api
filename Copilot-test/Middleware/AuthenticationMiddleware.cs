namespace Copilot_teste.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private const string ValidToken = "techhive-token-123";

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;

        if (path is not null && path.StartsWith("/swagger"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: token ausente.");
            return;
        }

        var token = authorizationHeader.ToString().Replace("Bearer ", "");

        if (token != ValidToken)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: token inválido.");
            return;
        }

        await _next(context);
    }
}