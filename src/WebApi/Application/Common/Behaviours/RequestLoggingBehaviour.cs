using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Common.Behaviours
{
    public class RequestLoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly GlobalSettings _globalSettings;
        
        public RequestLoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, GlobalSettings globalSettings)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _globalSettings = globalSettings;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            string userId = _currentUserService.UserId ?? string.Empty;
            string userName = _currentUserService.UserEmail;

            _logger.LogInformation("{ProjectName} Request: {Name} {UserId} {UserName} {@Request}",
                _globalSettings.ProjectName, requestName, userId, userName, request);
            
            return Task.CompletedTask;
        }
    }
}
