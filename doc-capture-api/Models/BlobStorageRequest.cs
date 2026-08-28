using System;
namespace doc_capture_api.Models
{
    public class BlobStorageRequest
    {
        public string FileId { get; set; }
        public string ContainerSource { get; set; }
        public string FileName { get; set; }
        public Stream Content { get; set; }
    }
}

