namespace Application.Interfaces.Service
{
    public interface IAiService
    {
        Task<string> GetResponseAsync(string userId,string role, string question);
    }
}
