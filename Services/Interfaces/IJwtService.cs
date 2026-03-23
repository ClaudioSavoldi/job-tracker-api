using JobTracker.API.Models;

namespace JobTracker.API.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
