using Tg_Bot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tg_Bot.Models;

namespace Tg_Bot.Contexts
{
    namespace EFCore.DataContexts
    {
    public class DataContexts : DbContext
    {
            public DbSet<Users> Users { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=students_db;Username=postgres;Password=381164957");
        }
    }
}
}
