using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LaserPointer.WebApi.WebApi.Common
{
    public class ServerSentEventsClient : IServerSentEventsClient
    {
        private readonly HttpResponse _response;

        internal ServerSentEventsClient(HttpResponse response)
        {
            _response = response;
        }

        public Task SendEventAsync(object msg)
        {
            return _response.WriteAsync(JsonSerializer.Serialize(msg));
        }

        public Task SendEventAsync(IList<object> msg)
        {
            var sendTasks = msg.Select(subMsg => _response.WriteAsync(JsonSerializer.Serialize(subMsg))).ToList();
            return Task.WhenAll(sendTasks);
        }
    }
}
