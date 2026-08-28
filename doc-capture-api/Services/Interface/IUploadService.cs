
using DocCapture.API.Models;

namespace DocCapture.API.Services
{
    public interface IUploadService
    {
        public Task<DocUploadResponse> UploadFile(DocUploadRequest docData, IFormFile file);
    }
}

