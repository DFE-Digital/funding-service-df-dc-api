
using doc_capture_api.Models;
using DocCapture.API.Models;

namespace DocCapture.API.Infrastructure
{
    public interface IBlobStorageClient
    {
        Task<BlobStorageResponse> UploadStorage(BlobStorageRequest request);
        Task<Stream> DownloadFromBlob(FileDownloadRequest request);
    }
}

