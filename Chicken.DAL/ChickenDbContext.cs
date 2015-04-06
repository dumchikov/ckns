using System.Data.Entity;
using Chicken.Domain.Models;

namespace Chicken.DAL
{
    public class ChickenDbContext : DbContext
    {
        public ChickenDbContext() : base("chicken") { }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Device> Devices { get; set; }
    }
}
