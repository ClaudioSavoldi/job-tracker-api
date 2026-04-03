using JobTracker.API.Dtos.Requests;
using JobTracker.API.Models;
using JobTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JobTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        public AuthController(UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        //Chiamata per registrazione utenti
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
            if (existingUser != null)
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

        //Chiamata per login utenti
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            //controllo se il dto ricevuto rispetta le regole di validazione               
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //controllo se la mail esiste nel database
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            //controllo se la password è corretta con metodo di identity
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = await _jwtService.GenerateTokenAsync(user);

            return Ok(new {token});

        }

      
    


    }
}
