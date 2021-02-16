using MessengerApp.Data;
using MessengerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessengerApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MessengerDbContext _context;

        public HomeController(MessengerDbContext context) => _context = context;

        public IActionResult Index()
        {
            var chats = _context.Chats
                .Include(x => x.Users)
                .Where(u => !u.Users.Any(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();

            return View(chats);
        }

        public IActionResult Find()
        {
            var users = _context.Users
                .Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ToList();

            return View(users);
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            var chat = _context.Chats
                .Include(m => m.Messages)
                .FirstOrDefault(x => x.Id == id);
           
            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string content)
        {
            var message = new Message
            {
                ChatId = chatId,
                Content = content,
                Nick = User.Identity.Name,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chatId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.Admin
            });

            _context.Chats.Add(chat);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var chatUser = new ChatUser
            {
                ChatId = id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.Member
            };
            _context.ChatUsers.Add(chatUser);

            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", "Home", new { id = id});
        }

        public IActionResult Private()
        {
            var chats = _context.Chats
                .Include(u => u.Users)
                    .ThenInclude(x => x.User)
                .Where(t => t.Type == ChatType.Private && t.Users
                    .Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();

            return View(chats);
        }

        public async  Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var chat = new Chat
            {
                Type = ChatType.Private
            };

            chat.Users.Add(new ChatUser {
                UserId = userId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            });

            _context.Chats.Add(chat);

            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chat.Id});
        }
    }
}
