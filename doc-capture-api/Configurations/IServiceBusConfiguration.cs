using System;
namespace DocCapture.API.Configurations
{
    public interface IServiceBusConfiguration
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }

}

