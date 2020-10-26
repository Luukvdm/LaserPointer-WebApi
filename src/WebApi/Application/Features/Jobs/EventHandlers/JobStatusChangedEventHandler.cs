﻿using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Application.Features.Jobs.SseService;
using LaserPointer.WebApi.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Features.Jobs.EventHandlers
{
    public class JobStatusChangedEventHandler : INotificationHandler<DomainEventNotification<JobStatusChangedEvent>>
    {
        private readonly ILogger<JobStatusChangedEventHandler> _logger;
        private readonly IServerSentEventsService _sse;

        public JobStatusChangedEventHandler(ILogger<JobStatusChangedEventHandler> logger, IServerSentEventsService sse)
        {
            _logger = logger;
            _sse = sse;
        }

        public Task Handle(DomainEventNotification<JobStatusChangedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("LaserPointer Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            var jEvent = new JobUpdateEvent(
                jobId: domainEvent.Job.Id,
               newStatus: domainEvent.NewStatus,
               oldStatus: domainEvent.OldStatus
            );
            _sse.SendEventAsync(jEvent.GetAsServerSentEvent());

            return Task.CompletedTask;
        }
    }
}
