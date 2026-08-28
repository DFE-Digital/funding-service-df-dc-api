
using doc_capture_api.Models;
using doc_capture_api.Services;
using DocCapture.API.Models;
using DocCapture.API.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocCapture.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IUploadService _fileUploadService;
        private readonly IStatusCheckService _statusCheckService;

        public FileUploadController(ILogger<FileUploadController> logger, IUploadService fileUploadService, IStatusCheckService statusCheckService)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _statusCheckService = statusCheckService;
    }

        


        [HttpPost]
        [ActionName("UploadFile")]
        [ProducesResponseType(typeof(DocUploadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [RequestSizeLimit(10485760)]
        public async Task<IActionResult> Upload([FromForm] DocUploadRequest request)
        {
            _logger.LogInformation($"Upload file Start");
            if (request.IsAllowed == false || !Enum.IsDefined(typeof(SourceSystems), request.SourceApplication))
                return BadRequest($"unkown {nameof(SourceSystems)} value specified");
            _logger.LogInformation($"Upload file request received for file {request.FileName} from {request.SourceApplication}");
            var result = await _fileUploadService.UploadFile(request, request.File);
            _logger.LogInformation($"Upload file complete");
            return Created(result.FileName,result);

        }

        [HttpGet]
        [ActionName("{fileId}/GetFileStatus")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFileStatus([FromRoute]string fileId)
        {
            _logger.LogInformation($"Retrieving file status for file {fileId}");
            var result = await _statusCheckService.GetFileStatus(fileId);
            return Ok(result);

        }
    }
}

