
namespace doc_capture_api.Configurations
{
    public class CosmosDBConfiguration : ICosmosDbConfiguration
    {
        public CosmosDBConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration.Bind("CosmosDb", this);
        }

        public string Account { get; set; }

        public string Key { get; set; }

        public string DatabaseName { get; set; }

        public string ContainerName { get; set; }
    }
}

