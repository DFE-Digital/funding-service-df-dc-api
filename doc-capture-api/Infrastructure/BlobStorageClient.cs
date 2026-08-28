
using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using doc_capture_api.Models;
using DocCapture.API.Configurations;
using DocCapture.API.Models;

namespace DocCapture.API.Infrastructure
{
    public class BlobStorageClient : IBlobStorageClient
    {
        private readonly ILogger<BlobStorageClient> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IStorageConfiguration _storageConfiguration;

        public BlobStorageClient(ILogger<BlobStorageClient> logger, IStorageConfiguration storageConfiguration, BlobServiceClient blobServiceClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storageConfiguration =
                storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BlobStorageResponse> UploadStorage(BlobStorageRequest request)
        {

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_getSourceContainer(request.ContainerSource));


            await blobContainer.CreateIfNotExistsAsync();
            var blobClient = blobContainer.GetBlobClient(request.FileName);

            IDictionary<string, string> metadata =
                new Dictionary<string, string>
                {
                    { "fileId", request.FileId },
                    {"sourceSystem", request.ContainerSource}
                };
            BlobUploadOptions options = new()
            {
                Metadata = metadata,
                
            };
            var response = await blobClient.UploadAsync(request.Content, options);

            if (response.GetRawResponse().Status != (int)HttpStatusCode.Created)
            {
                _logger.LogError($"BlobStorageClient.Upload  {request.FileName} Blob update status code is {response.GetRawResponse().Status}");
                throw new Exception(response.GetRawResponse().ReasonPhrase);
            }
            _logger.LogInformation($"BlobStorageClient.Upload of {request.FileName} is success.");

            return new BlobStorageResponse
            {
                
                    FileName = request.FileName,
                    FilePathUrl = blobClient.Uri.ToString()
                
            };
        }

        public async Task<Stream> DownloadFromBlob(FileDownloadRequest request)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_getSourceContainer(request.SourceSystem));
            BlobClient blobClient = blobContainerClient.GetBlobClient(request.FilePath);

            if (await blobClient.ExistsAsync())
            {
                MemoryStream file = new();
                var response = await blobClient.DownloadToAsync(file);
                file.Position = 0;
                return file;
            }
            throw new FileNotFoundException($"Given file path not found in {request.SourceSystem}");
        }

        private string _getSourceContainer(string containerSource)
        {
            foreach (var sourceMap in _storageConfiguration.Containers.Split("|"))
            {
                if (sourceMap.Split(":")[0] == containerSource)
                    return sourceMap.Split(":")[1];
            }
            return String.Empty;
        }

        
    }
}

