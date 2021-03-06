﻿using System.Text.Json;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.WebApi.Sse;
using Microsoft.AspNetCore.Http;

namespace LaserPointer.WebApi.WebApi.Common
{
    public class ClientEventDispatcher : IClientEventDispatcher
    {
        private readonly HttpResponse _response;

        internal ClientEventDispatcher(HttpResponse response)
        {
            _response = response;
        }

        public Task SendEventAsync(object msg)
        {
            return _response.WriteAsync(JsonSerializer.Serialize(msg));
        }

        public Task SendEventAsync(IClientEvent msg)
        {
            return _response.WriteSseEventAsync(msg);
        }
    }
}
