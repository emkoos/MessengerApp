using MessengerApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var chats = _context.Chats.ToList();

            return View(chats);
        }
    }
}
