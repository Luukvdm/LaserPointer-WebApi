using System;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using LaserPointer.IdentityServer.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;

namespace LaserPointer.IdentityServer.Infrastructure.Identity
{
    /// <summary>
    /// Configures the HttpContext by assigning IdentityServerOrigin.
    /// </summary>
    public class PublicFacingUrlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GlobalSettings _globalSettings;

        public PublicFacingUrlMiddleware(RequestDelegate next, GlobalSettings globalSettings)
        {
            _next = next;
            _globalSettings = globalSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            string baseUrl = _globalSettings.BaseUrl;
            string basePath = _globalSettings.BasePath;

            context.SetIdentityServerOrigin(baseUrl);
            context.SetIdentityServerBasePath(basePath/*request.PathBase.Value.TrimEnd('/')*/);

            await _next(context);
        }
    }
}
