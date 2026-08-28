
namespace doc_capture_api.Configurations
{
    public interface ICosmosDbConfiguration
    {
        string Account { get; set; }

        string Key { get; set; }

        string DatabaseName { get; set; }

        string ContainerName { get; set; }

    }
    public class CollectionInfo
    {
        public string Name { get; set; }
        public string PartitionKey { get; set; }
        public bool Default { get; set; }
    }
}

