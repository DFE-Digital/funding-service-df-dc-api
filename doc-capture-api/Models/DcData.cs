
using Newtonsoft.Json;

namespace doc_capture_api.Models
{
    public class DcData
    {
      
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "file")]
        public FileInfo File { get; set; }
    }

    public class FileInfo
    {
        public string FileName { get; set; }
        public string ScanStatus { get; set; }

    }
}

