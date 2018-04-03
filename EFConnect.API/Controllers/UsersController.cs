using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EFConnect.Contracts;
using EFConnect.Helpers;
using EFConnect.Models;
using EFConnect.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFConnect.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(UserParams userParams)
        {
            var users = await _userService.GetUsers(userParams);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdate userForUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _userService.GetUser(id);

            if (userFromRepo == null)
                return NotFound($"User could not be found.");

            if (currentUserId != userFromRepo.Id)
                return Unauthorized();

            if (await _userService.UpdateUser(id, userForUpdate))
                return NoContent();

            throw new Exception($"Updating user failed on save.");
        }
    }
}