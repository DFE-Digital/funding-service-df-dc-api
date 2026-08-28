
using Microsoft.Extensions.Configuration;

namespace DocCapture.API.Configurations
{
    public class ServiceBusConfiguration :IServiceBusConfiguration
    {
        public ServiceBusConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration.Bind("ServiceBus", this);
        }

        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}

