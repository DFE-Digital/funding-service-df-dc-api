using System;
namespace DocCapture.API.Infrastructure
{
    public interface IMyServiceBusClient
    {
        Task<bool> SendMessageAsync(string message);
    }
}

