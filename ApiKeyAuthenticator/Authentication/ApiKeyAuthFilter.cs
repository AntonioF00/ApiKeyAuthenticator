using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiKeyAuthenticator.Authentication
{
    public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
    {
        private readonly  IConfiguration            _configuration;
        private readonly  ILogger<ApiKeyAuthFilter> _logger;

        public ApiKeyAuthFilter(IConfiguration configuration, ILogger<ApiKeyAuthFilter> logger)
        {
            _configuration  = configuration;
            _logger         = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Api Key Missing");
                _logger.LogError($"{DateTime.Now} ~ StatusCode : {context.HttpContext.Response.StatusCode} ~ Error: Api Key Missing...");
                return;
            }

            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Invalid Api Key");
                _logger.LogError($"{DateTime.Now} ~ StatusCode : {context.HttpContext.Response.StatusCode} ~ Error: Invalid Api Key...");
                return;
            }
        }
    }
}
