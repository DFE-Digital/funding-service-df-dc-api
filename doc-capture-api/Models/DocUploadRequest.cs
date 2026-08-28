
namespace DocCapture.API.Models
{
    public class DocUploadRequest
    {
        public string FileName { get; set; }
        public string SourceApplication { get; set; }
        public string FileType { get; set; }
        public bool IsAllowed { get; set; }
        public IFormFile File { get; set; }
    }
}

