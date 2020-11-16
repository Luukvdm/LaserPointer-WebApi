using System;
using System.Threading.Tasks;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IClientEventService
    {
        Guid AddClient(IClientEventDispatcher clientEventDispatcher);
        IClientEventDispatcher RemoveClient(Guid clientId);
        Task SendEventAsync(IClientEvent msg, Guid? clientId);
        Task SendEventAsync(IClientEvent msg);
    }
}
