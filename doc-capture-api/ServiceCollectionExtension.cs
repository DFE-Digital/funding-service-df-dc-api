

using doc_capture_api.Configurations;
using doc_capture_api.Data;
using doc_capture_api.Services;
using DocCapture.API.Configurations;
using DocCapture.API.Infrastructure;
using DocCapture.API.Services;
using Microsoft.Azure.Cosmos;

namespace DocCapture.API
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IStatusCheckService, StatusCheckService>();


        }
        public static void AddConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<IStorageConfiguration, StorageConfiguration>();
            services.AddSingleton<IServiceBusConfiguration, ServiceBusConfiguration>();
            
            
        }

        private static  CosmosDbService InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            
            var account = configurationSection["connectionString"];
            var client = new CosmosClient(account, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway // ConnectionMode.Direct is the default
            });
            var cosmosDbService = new CosmosDbService(client);
            
            return cosmosDbService;
        }
        public static void AddInfrastuctureServices(this IServiceCollection services, IConfiguration configuration)
        {            
            services.AddSingleton<IBlobStorageClient, BlobStorageClient>();
            services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(configuration.GetSection("CosmosDb")));
            services.AddSingleton<IMyServiceBusClient, MyServiceBusClient>();
        }




    }
}

