﻿using System;
using System.Threading.Tasks;

namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IServerSentEventsService
    {
        Guid AddClient(IServerSentEventsClient client);
        IServerSentEventsClient RemoveClient(Guid clientId);
        Task SendEventAsync(IServerSentEvent msg, Guid? clientId);
        Task SendEventAsync(IServerSentEvent msg);
    }
}
