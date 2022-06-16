using AppTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AppTracker.Middleware
{
    public class TokenAuthentication
    {
        private RequestDelegate _next { get; }
        private IOptionsMonitor<BuildSettingsOptions> _options { get; }
        public TokenAuthentication(RequestDelegate next, IOptionsMonitor<BuildSettingsOptions> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {

        }

    }
}
