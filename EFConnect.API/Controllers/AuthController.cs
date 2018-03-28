using System.Threading.Tasks;
using EFConnect.Contracts;
using EFConnect.Data.Entities;
using EFConnect.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace EFConnect.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegister userForRegister)
        {
            userForRegister.Username = userForRegister.Username.ToLower();

            if (await _authService.UserExists(userForRegister.Username))
                return BadRequest("Username is already taken");
            
            var userToCreate = new User
            {
                Username = userForRegister.Username
            };

            var createUser = await _authService.Register(userToCreate, userForRegister.Password);

            return StatusCode(201);
        }
    }
}