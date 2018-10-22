using Microsoft.EntityFrameworkCore;
using NebulaChat.Core.Models;

namespace NebulaChat.Data
{
    public class NebulaChatDbContext : DbContext
    {
        public NebulaChatDbContext(DbContextOptions<NebulaChatDbContext> options)
            : base(options)
        { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
