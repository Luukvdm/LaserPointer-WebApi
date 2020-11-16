using System.Threading.Tasks;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IClientEventDispatcher
    {
        Task SendEventAsync(IClientEvent msg);
    }
}
