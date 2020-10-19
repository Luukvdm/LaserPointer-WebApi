using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly GlobalSettings _globalSettings;

        public PerformanceBehaviour(
            ILogger<TRequest> logger, 
            ICurrentUserService currentUserService, GlobalSettings globalSettings)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserService = currentUserService;
            _globalSettings = globalSettings;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            long elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                string requestName = typeof(TRequest).Name;
                string userId = _currentUserService.UserId ?? string.Empty;
                string userName = string.Empty;

                if (!string.IsNullOrEmpty(userId))
                {
                    userName = _currentUserService.UserEmail;
                }

                _logger.LogWarning("{ProjectName} Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    _globalSettings.ProjectName, requestName, elapsedMilliseconds, userId, userName, request);
            }

            return response;
        }
    }
}
