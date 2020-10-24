using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.WebApi.Services
{
    public class ServerSentEventsService : IServerSentEventsService
    {
        private readonly ConcurrentDictionary<Guid, IServerSentEventsClient> _clients;
        private readonly ILogger<ServerSentEventsService> _logger;

        public ServerSentEventsService(ILogger<ServerSentEventsService> logger)
        {
            _logger = logger;
            _clients = new ConcurrentDictionary<Guid, IServerSentEventsClient>();
        }

        public Guid AddClient(IServerSentEventsClient client)
        {
            var clientId = Guid.NewGuid();
            _clients.TryAdd(clientId, client);
            return clientId;
        }

        public IServerSentEventsClient RemoveClient(Guid clientId)
        {
            _clients.TryRemove(clientId, out var client);
            return client;
        }

        public Task SendEventAsync(ServerSentEvent msg)
        {
            if (msg.ClientId.HasValue)
            {
                _logger.LogDebug($"Sending SSE to client id {msg.ClientId.Value}");
                return _clients[msg.ClientId.Value].SendEventAsync(msg.Message);
            }

            _logger.LogDebug($"Sending SSE to all {_clients.Count} clients");
            var clientTasks = _clients.Select(client => client.Value.SendEventAsync(msg.Message)).ToList();
            return Task.WhenAll(clientTasks);
        }
    }
}
