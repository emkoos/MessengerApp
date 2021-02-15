using MessengerApp.Data;
using MessengerApp.Hubs;
using MessengerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _chat;

        public ChatController(IHubContext<ChatHub> chat)
        {
            _chat = chat;    
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinChat(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);
            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> LeaveChat(string connectionId, string roomName)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(string content, string roomName, int chatId, [FromServices]MessengerDbContext context)
        {
            var message = new Message
            {
                ChatId = chatId,
                Content = content,
                Nick = User.Identity.Name,
                Timestamp = DateTime.Now
            };

            context.Messages.Add(message);
            await context.SaveChangesAsync();

            await _chat.Clients.Group(roomName)
                .SendAsync("ReceiveMessage", message);

            return Ok();
        }
    }
}
