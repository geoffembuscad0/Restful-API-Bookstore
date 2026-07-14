using RestfulApiDemo.Services;

namespace RestfulApiDemo.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISessionService _sessionService;

        public AuthenticationMiddleware(RequestDelegate next, ISessionService sessionService)
        {
            _next = next;
            _sessionService = sessionService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // List of endpoints that don't require authentication
            var publicEndpoints = new[] 
            { 
                "/api/auth/register",
                "/api/auth/login",
                "/swagger",
                "/swagger/index.html",
                "/swagger/swagger-ui.css",
                "/swagger/swagger-ui-bundle.js",
                "/swagger/swagger-ui-standalone-preset.js",
                "/swagger/swagger-initializer.js"
            };

            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Check if the current path is public
            bool isPublicEndpoint = publicEndpoints.Any(endpoint => path.StartsWith(endpoint.ToLower()));

            if (!isPublicEndpoint)
            {
                // Get authorization header
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Authorization header is missing" });
                    return;
                }

                // Extract token from "Bearer <token>"
                var token = authHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();

                if (!_sessionService.IsSessionValid(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Invalid or expired session token" });
                    return;
                }

                // Store token in context for later use
                context.Items["SessionToken"] = token;
            }

            await _next(context);
        }
    }
}