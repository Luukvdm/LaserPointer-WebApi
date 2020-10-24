using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IServerSentEventsClient
    {
        Task SendEventAsync(IList<object> msg);
    }
}
