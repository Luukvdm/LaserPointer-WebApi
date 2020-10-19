using System;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly GlobalSettings _globalSettings;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger, GlobalSettings globalSettings)
        {
            _logger = logger;
            _globalSettings = globalSettings;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                string requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "{ProjectName} Request: Unhandled Exception for Request {Name} {@Request}", 
                    _globalSettings.ProjectName, requestName, request);

                throw;
            }
        }
    }
}
