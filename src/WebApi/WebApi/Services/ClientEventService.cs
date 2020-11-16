using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.WebApi.Services
{
    public class ClientEventService : IClientEventService
    {
        private readonly ConcurrentDictionary<Guid, IServerSentEventsClient> _clients;
        private readonly ILogger<ClientEventService> _logger;
        private readonly GlobalSettings _globalSettings;

        public ClientEventService(ILogger<ClientEventService> logger, GlobalSettings globalSettings)
        {
            _logger = logger;
            _globalSettings = globalSettings;
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

        public async Task SendEventAsync(IClientEvent msg)
        {
            await SendEventAsync(msg, null);
        }

        public async Task SendEventAsync(IClientEvent msg, Guid? clientId)
        {
            if (clientId.HasValue)
            {
                _logger.LogInformation($"Sending SSE to client id {clientId.Value}");
                await _clients[clientId.Value].SendEventAsync(msg);
                return;
            }

            _logger.LogInformation("{ProjectName} SSE event: {MessageType} {ClientCount}",
                _globalSettings.ProjectName, msg.Type, _clients.Count);
            var clientTasks = _clients.Select(client => client.Value.SendEventAsync(msg)).ToList();
            await Task.WhenAll(clientTasks);
        }
    }
}
