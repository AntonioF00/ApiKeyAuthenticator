namespace ApiKeyAuthenticator.Authentication
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate                _next;
        private          ILogger<ApiKeyAuthMiddleware>  _logger;
        private readonly IConfiguration                 _configuration;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyAuthMiddleware> logger)
        {
            _configuration  = configuration;
            _logger         = logger;
            _next           = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key missing...");
                _logger.LogError($"{DateTime.Now} ~ StatusCode : {context.Response.StatusCode} ~ Error: Api Key Missing...");
                return;
            }

            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Api Key...");
                _logger.LogError($"{DateTime.Now} ~ StatusCode : {context.Response.StatusCode} ~ Error: Invalid Api Key...");
                return;
            }

            await _next(context);
        }
    }
}
