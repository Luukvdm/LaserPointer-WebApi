using System;
using System.Collections.Generic;
using LaserPointer.WebApi.Application.Common.Interfaces;

namespace LaserPointer.WebApi.Application.Common.Models
{
    public class ServerSentEvent : IServerSentEvent
    {
        public ServerSentEvent(string type, params object[] data)
        {
            Id = Guid.NewGuid().ToString();
            Data = data;
            Type = type;
        }
        public string Id { get; set; }
        public string Type { get; set; }
        public IList<object> Data { get; }
    }
}
