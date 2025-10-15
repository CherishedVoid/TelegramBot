using Tg_Bot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tg_Bot.Contexts.EFCore.DataContexts
{
    public class DataContexts : DbContext
    {
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<DataContexts>()
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found in secrets.json");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}