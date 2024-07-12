namespace MolyCoreWeb.Services
{
    public interface IDownloadService
    {
        Task DownloadFileAsync(string url, string outputPath);
    }
}
