using Microsoft.Extensions.Configuration;

namespace DocCapture.API.Configurations
{
    public class StorageConfiguration : IStorageConfiguration
    {
        public StorageConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration.Bind("Storage", this);
        }

        public string ConnectionString { get; set; }
        public string Containers { get; set; }
    }
}