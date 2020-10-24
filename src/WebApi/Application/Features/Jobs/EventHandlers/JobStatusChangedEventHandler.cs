using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Features.Jobs.EventHandlers
{
    public class JobStatusChangedEventHandler : INotificationHandler<DomainEventNotification<JobStatusChangedEvent>>
    {
        private readonly ILogger<JobStatusChangedEventHandler> _logger;

        public JobStatusChangedEventHandler(ILogger<JobStatusChangedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<JobStatusChangedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("LaserPointer Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
