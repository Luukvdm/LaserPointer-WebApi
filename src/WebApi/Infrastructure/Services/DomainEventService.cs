using System;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        private readonly IMediator _mediator;
        private readonly GlobalSettings _globalSettings;

        public DomainEventService(ILogger<DomainEventService> logger, IMediator mediator, GlobalSettings globalSettings)
        {
            _logger = logger;
            _mediator = mediator;
            _globalSettings = globalSettings;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}
