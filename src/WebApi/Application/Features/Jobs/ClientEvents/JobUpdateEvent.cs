using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Application.Features.Jobs.ClientEvents
{
    public enum JobUpdateType
    {
        StatusChange,
        New
    }
    public static class JobUpdateEventExtension
    {
        public static ClientEvent GetAsClientEvent(this JobUpdateEvent jobUpdateEvent)
        {
            var type = jobUpdateEvent.OldStatus == null ? JobUpdateType.New : JobUpdateType.StatusChange;
            
            // Set first char to lower
            string strType = type.ToString();
            strType = char.ToLower(type.ToString()[0]) + strType.Substring(1);
            
            return new ClientEvent(strType, jobUpdateEvent);
        }
    }
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
