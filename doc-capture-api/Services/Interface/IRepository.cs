
using doc_capture_api.Models;

namespace doc_capture_api.Data
{
    public interface IRepository
    {
        Task<DcData> GetByIdAsync(string id);
    }
}

