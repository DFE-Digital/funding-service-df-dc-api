using System;
namespace doc_capture_api.Models
{
    public class FileDownloadRequest
    {
        public string FilePath { get; set; }
        public string SourceSystem { get; set; }
    }
}

