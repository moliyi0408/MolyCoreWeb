namespace MolyCoreWeb.Services
{
    public interface ILineNotifyService
    {
        Task SendMessageAsync(string message);
    }
}
