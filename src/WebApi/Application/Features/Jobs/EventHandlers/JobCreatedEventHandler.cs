using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Application.Features.Jobs.SseService;
using LaserPointer.WebApi.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Features.Jobs.EventHandlers
{
    public class JobCreatedEventHandler : INotificationHandler<DomainEventNotification<JobCreatedEvent>>
    {
        private readonly ILogger<JobCreatedEventHandler> _logger;
        private readonly IServerSentEventsService _sse;

        public JobCreatedEventHandler(ILogger<JobCreatedEventHandler> logger, IServerSentEventsService sse)
        {
            _logger = logger;
            _sse = sse;
        }

        public Task Handle(DomainEventNotification<JobCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("LaserPointer Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            var jEvent = new JobUpdateEvent(domainEvent.Job.Id);
            _sse.SendEventAsync(jEvent.GetAsServerSentEvent());

            return Task.CompletedTask;
        }
    }
}
