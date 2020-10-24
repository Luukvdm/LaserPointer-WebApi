using System;
using System.Collections.Generic;

namespace LaserPointer.WebApi.Application.Common.Models
{
    public class ServerSentEvent
    {
        public ServerSentEvent()
        {
            Message = new List<object>();
        }
        public Guid? ClientId { get; set; }
        public IList<object> Message { get; private set; }
    }
}
