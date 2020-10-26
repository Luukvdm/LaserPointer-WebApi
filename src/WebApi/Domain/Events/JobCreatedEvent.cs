using LaserPointer.WebApi.Domain.Common;
using LaserPointer.WebApi.Domain.Entities;

namespace LaserPointer.WebApi.Domain.Events
{
    public class JobCreatedEvent : DomainEvent
    {
        public JobCreatedEvent(Job job)
        {
            Job = job;
        }

        public Job Job { get; }
    }
}
