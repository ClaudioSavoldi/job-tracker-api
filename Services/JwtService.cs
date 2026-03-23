using JobTracker.API.Models;
using JobTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace JobTracker.API.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

    }
}
