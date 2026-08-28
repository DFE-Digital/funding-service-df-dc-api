using System;
namespace doc_capture_api.Services
{
    public interface IStatusCheckService
    {
        Task<string> GetFileStatus(string fileId);
    }
}

