using MessengerApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.Data
{
    public class MessengerDbContext : IdentityDbContext<User>
    {
        public MessengerDbContext(DbContextOptions<MessengerDbContext> options) 
            : base(options) { }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
