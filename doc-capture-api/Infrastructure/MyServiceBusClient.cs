using Azure.Messaging.ServiceBus;
using DocCapture.API.Configurations;
using System.Text;

namespace DocCapture.API.Infrastructure
{
    public class MyServiceBusClient : IMyServiceBusClient
    {
        private readonly ILogger<MyServiceBusClient> _logger;
        private readonly ServiceBusSender _serviceBusSender;
        private readonly Azure.Messaging.ServiceBus.ServiceBusClient _serviceBusClient;

        public MyServiceBusClient(ILogger<MyServiceBusClient> logger, IServiceBusConfiguration serviceBusConfiguration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (serviceBusConfiguration == null)
                throw new ArgumentNullException(nameof(serviceBusConfiguration));

            _serviceBusClient = new Azure.Messaging.ServiceBus.ServiceBusClient(serviceBusConfiguration.ConnectionString);
            _serviceBusSender = _serviceBusClient.CreateSender(serviceBusConfiguration.QueueName);
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                _logger.LogWarning("Attempted to send a null or empty message.");
                return false;
            }

            try
            {
                var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));
                await _serviceBusSender.SendMessageAsync(serviceBusMessage);
                _logger.LogInformation("Message sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message.");
                return false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _serviceBusSender.DisposeAsync();
            await _serviceBusClient.DisposeAsync();
        }
    }
}
