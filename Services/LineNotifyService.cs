
namespace MolyCoreWeb.Services
{
    public class LineNotifyService : ILineNotifyService
    {
        private readonly HttpClient _httpClient;
    
        public LineNotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        public async Task SendMessageAsync(string message)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(message), "message" }
            };

            var response = await _httpClient.PostAsync("api/notify", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
