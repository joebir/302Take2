using System.Security.Claims;
using System.Threading.Tasks;
using EFConnect.Contracts;
using EFConnect.Models.Photo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFConnect.API.Controllers
{
    [Authorize]
    [Route("Api/Users/{userId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;

        public PhotosController(IUserService userService, IPhotoService photoService)
        {
            _userService = userService;
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreation photoDto)
        {
            var user = await _userService.GetUser(userId);

            if (user == null)
                return BadRequest("Could not find user");

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (currentUserId != user.Id)
            {
                return Unauthorized();
            }

            var photoForReturn = await _photoService.AddPhotoForUser(userId, photoDto);

            if (photoForReturn != null)
                return CreatedAtRoute("GetPhoto", new { id = photoForReturn.Id }, photoForReturn);

            return BadRequest();
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await _photoService.GetPhoto(id);

            return Ok(photo);
        }
    }
}