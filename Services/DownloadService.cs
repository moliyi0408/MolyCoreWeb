namespace MolyCoreWeb.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly HttpClient _httpClient;

        public DownloadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DownloadFileAsync(string url, string outputPath)
        {
            // 下载文件
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            await using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);
        }
    }
}
