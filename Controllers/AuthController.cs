using JobTracker.API.Dtos.Requests;
using JobTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerDto)
        {
            //controllo se il dto ricevuto rispetta le regole di validazione               
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //controllo se la mail con cui ci si registra esiste gia
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if(existingUser != null)
            {
                return BadRequest("Email already exists!");
            }
            //creo l`utente
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            //controllo se l`utente è stato creato correttamente
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            //restituire successo
            return StatusCode(201, "User registered successfully!");

        }
    }
}
