using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Application.Features.Jobs.SseService
{
    public class JobUpdateEvent
    {
        /// <summary>
        /// Create UpdateEvent for a new job
        /// </summary>
        /// <param name="jobId">Job id</param>
        public JobUpdateEvent(int jobId)
        {
            JobId = jobId;
        }

        /// <summary>
        /// Create a UpdateEvent for a job status change
        /// </summary>
        /// <param name="jobId">Job id</param>
        /// <param name="oldStatus">Old status</param>
        /// <param name="newStatus">New status</param>
        public JobUpdateEvent(int jobId, JobStatus oldStatus, JobStatus newStatus)
        {
            JobId = jobId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }

        public int JobId { get; set; }
        public JobStatus? OldStatus { get; protected set; }
        public JobStatus? NewStatus { get; protected set; }

    }
}
