
using System.Text.Json;
using doc_capture_api.Models;
using DocCapture.API.Configurations;
using DocCapture.API.Infrastructure;
using DocCapture.API.Models;

namespace DocCapture.API.Services
{
    public class UploadService :IUploadService
    {
        private readonly IBlobStorageClient _blobStorageClient;
        private readonly IMyServiceBusClient _serviceBusClient;
        private readonly ILogger<UploadService> _logger;


        public UploadService(IBlobStorageClient blobStorageClient, IMyServiceBusClient serviceBusClient, ILogger<UploadService> logger)
        {
            _blobStorageClient = blobStorageClient;
            _serviceBusClient = serviceBusClient;
            _logger = logger;
        }

        public async Task<DocUploadResponse> UploadFile(DocUploadRequest docData, IFormFile file)
        {
            var fileId = Guid.NewGuid();
            string blobPath = "scanned/" + docData.FileName;
            _logger.LogInformation($"File path to upload: {blobPath}");
            try
            {
                // Step 1 : Insert to Blob Storage from docData

                // Pending : Add the FileId to the tags of Blob.

                var result = await _blobStorageClient.UploadStorage(new BlobStorageRequest()
                {
                    FileId = fileId.ToString(),
                    ContainerSource = docData.SourceApplication,
                    Content = file.OpenReadStream(),
                    FileName = blobPath
                });

                // Step 2 : If success, then update the response class-docUploadResponses
                //          and return
                if (result != null)
                {
                    _logger.LogInformation($"File uploaded successfully to {blobPath}.");
                    var message = new DocumentData()
                    {
                        FileId = fileId,
                        FileName = blobPath,
                        FileStatus = FileStatus.created.ToString(),
                        FileType = docData.FileType,
                        SourceSystem = docData.SourceApplication
                    };
                    _logger.LogInformation($"Sending Msg to SB to trigger the fnapp and update cosmos db.");
                    var sendMessage = await _serviceBusClient.SendMessageAsync(JsonSerializer.Serialize(message));

                }
                Thread.Sleep(2000);
                return new DocUploadResponse() { FileName = docData.FileName, FileId = fileId };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new DocUploadResponse() { FileName = $"{fileId} | {ex.Message}", FileId = null };
            }
            finally
            {

            }
            
        }

    }
}

