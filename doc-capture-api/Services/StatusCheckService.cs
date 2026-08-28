using doc_capture_api.Data;
using doc_capture_api.Models;
using Newtonsoft.Json;
using System;

namespace doc_capture_api.Services
{
    public class StatusCheckService : IStatusCheckService
    {
        private readonly ICosmosDbService _cosmosService;
        public StatusCheckService(ICosmosDbService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        public async Task<string> GetFileStatus(string fileId)
        {
            DocData result = await GetDCData(fileId);
            return result?.scanStatus??"unknown";

        }
        private async Task<DocData> GetDCData(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("DFSQLAPIKEY"));
                string Url = $"{Environment.GetEnvironmentVariable("DFSQLAPIURL")}/api/GetDocumentCapture/" + id;
                var response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                var auditLog = JsonConvert.DeserializeObject<DocData>(responseData);
                return auditLog;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}

