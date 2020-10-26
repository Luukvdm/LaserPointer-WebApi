using System.Collections.Generic;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IServerSentEvent
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public IList<object> Data { get; }
    }
}
