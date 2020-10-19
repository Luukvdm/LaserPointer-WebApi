using System.Threading.Tasks;
using LaserPointer.WebApi.Domain.Common;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
