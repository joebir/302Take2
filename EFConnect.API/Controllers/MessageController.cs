using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EFConnect.Contracts;
using EFConnect.Helpers;
using EFConnect.Models;
using EFConnect.Models.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFConnect.API.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/users/{userId}/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessagesController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _messageService.GetMessage(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, [FromBody] MessageForCreation messageForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreation.SenderId = userId;

            var messageToReturn = await _messageService.CreateMessage(messageForCreation);

            if (messageToReturn != null)
                return CreatedAtRoute("GetMessage", new { id = messageToReturn.Id }, messageToReturn);

            throw new Exception("Creating the message failed on save.");
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messages = await _messageService.GetMessagesForUser(messageParams);

            Response.AddPagination(
                    messages.CurrentPage,
                    messages.PageSize,
                    messages.TotalCount,
                    messages.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{id}")]
        public async Task<IActionResult> GetMessageThread(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messages = await _messageService.GetMessageThread(userId, id);

            return Ok(messages);
        }
    }
}