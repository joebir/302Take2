using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EFConnect.Contracts;
using EFConnect.Data.Entities;
using EFConnect.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EFConnect.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService, IConfiguration config)
        {
            _config = config;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegister userForRegister)
        {
            if (!string.IsNullOrEmpty(userForRegister.Username))
                userForRegister.Username = userForRegister.Username.ToLower();

            if (await _authService.UserExists(userForRegister.Username))
                ModelState.AddModelError("Username", "Username already exists");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userToCreate = new User
            {
                Username = userForRegister.Username
            };

            var createUser = await _authService.Register(userToCreate, userForRegister.Password);

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLogin userForLogin)
        {
            var userFromDb = await _authService.Login(userForLogin.Username.ToLower(), userForLogin.Password);

            if (userFromDb == null)
                return Unauthorized();
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromDb.Username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok( new { tokenString });
        }
    }
}