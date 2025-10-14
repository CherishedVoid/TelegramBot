using Tg_Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace Tg_Bot
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=username_db;Username=postgres;Password=381164957");
        }
    }
}