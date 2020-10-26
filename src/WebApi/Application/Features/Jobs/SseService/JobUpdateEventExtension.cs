using LaserPointer.WebApi.Application.Common.Models;

namespace LaserPointer.WebApi.Application.Features.Jobs.SseService
{
    public static class JobUpdateEventExtension
    {
        public static ServerSentEvent GetAsServerSentEvent(this JobUpdateEvent jobUpdateEvent)
        {
            var type = jobUpdateEvent.OldStatus == null ? JobUpdateType.New : JobUpdateType.StatusChange;
            
            // Set first char to lower
            string strType = type.ToString();
            strType = char.ToLower(type.ToString()[0]) + strType.Substring(1);
            
            return new ServerSentEvent(strType, jobUpdateEvent);
        }
    }
}
