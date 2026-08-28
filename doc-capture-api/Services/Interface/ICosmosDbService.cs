using doc_capture_api.Models;

public interface ICosmosDbService
{
    Task<DcData> GetAsync(string id);
    
}