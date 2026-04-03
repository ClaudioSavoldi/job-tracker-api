using JobTracker.API.Models;
using JobTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            //se l’utente ha ruoli, vogliamo inserirli nei claim del token
            var roles = await _userManager.GetRolesAsync(user);

            //inserisco info utili nel token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
            };
            //posso inserire piu ruoli per un solo utente
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //leggo la string chiave dal config e la trasformo in byte, trasformandola poi nell oggetto key che uso per firmare
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            
            //firmo questo token usando la key e questo algoritmo sicuro
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            /*creo loggetto token in memoria:
             * issuer: chi ha emesso il token
             * audience: che è il destinatario del token
             * claims: iserisco le info create nelle righe precedenti
             * expires: tra quanto scade
             * signingCredentials: firmo con key e algoritmo
            */
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
                );

            //Prende l’oggetto JwtSecurityToken e lo converte nella classica stringa JWT da restituire al client.
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
