using ApiKeyAuthenticator.Authentication;

namespace ApiKeyAuthenticator.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiAuthMiddleware(this IApplicationBuilder app) => app.UseMiddleware<ApiKeyAuthMiddleware>();

        public static IServiceCollection UseApiAuthFilter(this WebApplicationBuilder builder) => builder.Services.AddScoped<ApiKeyAuthFilter>();

    }
}
