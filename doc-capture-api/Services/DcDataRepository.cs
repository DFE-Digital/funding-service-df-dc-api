using System;
using System.Net;
using doc_capture_api.Models;
using Microsoft.Azure.Cosmos;

namespace doc_capture_api.Data
{
    public class DcDataRepository : IRepository
    {
        private readonly ICosmosDbClientFactory _cosmosDbClientFactory;
        public DcDataRepository(ICosmosDbClientFactory cosmosDbClientFactory)
        {
            _cosmosDbClientFactory = cosmosDbClientFactory ?? throw new ArgumentNullException(nameof(cosmosDbClientFactory));
        }

        public async Task<DcData> GetByIdAsync(string id)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetContainer("dc-data");
                var itemResponse = await cosmosDbClient.ReadItemAsync<DcData>(id, new PartitionKey(id));

                if (itemResponse == null || itemResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new CosmosException($"{id} not found", HttpStatusCode.NotFound, 100, null, 0);
                }
                return itemResponse.Resource;
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception(e.Message);
                }

                throw;
            }
        }
    }
}

