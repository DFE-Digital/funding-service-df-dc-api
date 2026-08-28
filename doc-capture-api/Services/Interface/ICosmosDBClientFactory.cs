
using Microsoft.Azure.Cosmos;

namespace doc_capture_api.Data
{
    public interface ICosmosDbClientFactory
    {
        Container GetContainer(string collectionName);

    }
}

