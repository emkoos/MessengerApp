using MessengerApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessengerApp.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private readonly MessengerDbContext _context;

        public RoomViewComponent(MessengerDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var chats = _context.ChatUsers
                .Include(x => x.Chat)
                .Where(x => x.UserId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value 
                                                            && x.Chat.Type == Models.ChatType.Room)
                .Select(c => c.Chat)
                .ToList();

            return View(chats);
        }
    }
}
