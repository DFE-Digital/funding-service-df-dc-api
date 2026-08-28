
using Microsoft.Azure.Cosmos;

namespace doc_capture_api.Data
{
    public class CosmosDbClientFactory : ICosmosDbClientFactory
    {
        private readonly string _databaseName;
        private readonly CosmosClient _cosmosClient;


        public CosmosDbClientFactory(string databaseName, CosmosClient cosmosClient)
        {
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
        }

        public Container GetContainer(string collectionName)
        {
            return _cosmosClient.GetContainer(_databaseName, collectionName);
        }


        public async Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync()
        {
            return await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName);

        }

    }
}
