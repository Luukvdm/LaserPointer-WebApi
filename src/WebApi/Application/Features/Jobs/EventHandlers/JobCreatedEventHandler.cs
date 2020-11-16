﻿using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Application.Features.Jobs.ClientEvents;
using LaserPointer.WebApi.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Application.Features.Jobs.EventHandlers
{
    public class JobCreatedEventHandler : INotificationHandler<DomainEventNotification<JobCreatedEvent>>
    {
        private readonly ILogger<JobCreatedEventHandler> _logger;
        private readonly IServerSentEventsService _sse;
        private readonly ICurrentUserService _currentUser;
        private readonly GlobalSettings _globalSettings;

        public JobCreatedEventHandler(ILogger<JobCreatedEventHandler> logger, IServerSentEventsService sse, GlobalSettings globalSettings, ICurrentUserService currentUser)
        {
            _logger = logger;
            _sse = sse;
            _globalSettings = globalSettings;
            _currentUser = currentUser;
        }

        public Task Handle(DomainEventNotification<JobCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("{ProjectName} Domain Event: {DomainEventType} {JobId} {UserId} {UserName} {UserEmail}",
                _globalSettings.ProjectName, domainEvent.GetType().Name, domainEvent.Job.Id, _currentUser.UserId, _currentUser.UserName, _currentUser.UserEmail);

            var jEvent = new JobUpdateEvent(domainEvent.Job.Id);
            _sse.SendEventAsync(jEvent.GetAsServerSentEvent());

            return Task.CompletedTask;
        }
    }
}
