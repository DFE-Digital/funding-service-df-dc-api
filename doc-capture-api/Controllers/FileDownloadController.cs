
using doc_capture_api.Models;
using DocCapture.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace doc_capture_api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileDownloadController
    {
        private readonly ILogger<FileDownloadController> _logger;
        private readonly IBlobStorageClient _blobStorageClient;

        public FileDownloadController(ILogger<FileDownloadController> logger, IBlobStorageClient blobStorageClient)
        {
            _logger = logger;
            _blobStorageClient = blobStorageClient;
        }

        [HttpPost]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ActionName("downloadFile")]
        [DisableRequestSizeLimit()]
        public async Task<FileStreamResult> DownloadFile([FromForm] FileDownloadRequest request)
        {
            _logger.LogInformation($"Download file start");
            request.FilePath = "scanned/" + request.FilePath; // Upload service will return filepath will not include scanned / quarentine folder indication.
            _logger.LogInformation($"Download file request receied for file path: {request.FilePath} from {request.SourceSystem}.");
            var result = await _blobStorageClient.DownloadFromBlob(request);
            _logger.LogInformation($"Download file complete");
            return new FileStreamResult(result, new MediaTypeHeaderValue("application/octet-stream"))
            {
                FileDownloadName = request.FilePath.Split('/').Last()
            };

        }
    }
}

