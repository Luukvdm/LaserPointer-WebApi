using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Common.Behaviours
{
    public class NotificationLoggingService<TNotification> : INotificationHandler<TNotification> 
        where TNotification : INotification
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly GlobalSettings _globalSettings;

        public NotificationLoggingService(ILogger<TNotification> logger, ICurrentUserService currentUserService, GlobalSettings globalSettings)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _globalSettings = globalSettings;
        }

        public Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            /* _logger.LogInformation("{ProjectName} Domain Event: {UserId} {UserName} {NotificationType}",
                _globalSettings.ProjectName, 
                _currentUserService.UserId, _currentUserService.UserName,
                notification.GetType()
                );
            */
            return Task.CompletedTask;
        }
    }
}
