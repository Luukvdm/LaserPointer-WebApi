using System;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Models;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IServerSentEventsService
    {
        Guid AddClient(IServerSentEventsClient client);
        IServerSentEventsClient RemoveClient(Guid clientId);
        Task SendEventAsync(ServerSentEvent msg);
    }
}
