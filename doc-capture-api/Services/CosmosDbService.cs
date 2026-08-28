using System;
using doc_capture_api.Models;
using Microsoft.Azure.Cosmos;

namespace doc_capture_api.Data
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        public CosmosDbService(
            CosmosClient cosmosDbClient
            )
        {
            _container = cosmosDbClient.GetContainer("document-capture-db", "dc-data");
        }
        
        public async Task<DcData> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<DcData>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) //For handling item not found and other exceptions
            {
                return new DcData() { Id = "failure"+ex.Message};
            }
        }
    }
}

