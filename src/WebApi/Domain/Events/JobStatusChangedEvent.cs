using LaserPointer.WebApi.Domain.Common;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Domain.Events
{
    public class JobStatusChangedEvent : DomainEvent
    {
        public JobStatusChangedEvent(Job job, JobStatus newStatus, JobStatus oldStatus)
        {
            Job = job;
            NewStatus = newStatus;
            OldStatus = oldStatus;
        }

        public Job Job { get; }
        public JobStatus NewStatus { get; }
        public JobStatus OldStatus { get; }
    }
}
