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
    public class JobStatusChangedEventHandler : INotificationHandler<DomainEventNotification<JobStatusChangedEvent>>
    {
        private readonly ILogger<JobStatusChangedEventHandler> _logger;
        private readonly IClientEventService _clientEventService;
        private readonly ICurrentUserService _currentUser;
        private readonly GlobalSettings _globalSettings;

        public JobStatusChangedEventHandler(ILogger<JobStatusChangedEventHandler> logger, IClientEventService clientEventService, ICurrentUserService currentUser, GlobalSettings globalSettings)
        {
            _logger = logger;
            _clientEventService = clientEventService;
            _currentUser = currentUser;
            _globalSettings = globalSettings;
        }

        public Task Handle(DomainEventNotification<JobStatusChangedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            
            _logger.LogInformation("{ProjectName} Domain Event: {DomainEventType} {JobId} {UserId} {UserName}",
                _globalSettings.ProjectName, domainEvent.GetType().Name, domainEvent.Job.Id, 
                _currentUser.UserId, _currentUser.UserName);

            var jEvent = new JobUpdateEvent(
                jobId: domainEvent.Job.Id,
                newStatus: domainEvent.NewStatus,
                oldStatus: domainEvent.OldStatus
            );
            _clientEventService.SendEventAsync(jEvent.GetAsClientEvent());

            return Task.CompletedTask;
        }
    }
}
