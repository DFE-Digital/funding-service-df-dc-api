using System;
namespace DocCapture.API.Models
{
    public class DocUploadResponse
    {
        public Guid? FileId { get; set; }

        public string FileName { get; set; }
    }
}

