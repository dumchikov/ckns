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

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post>()
        //                .HasMany<Attachment>(s => s.Attachments)
        //                .WithRequired(s => s.Post)
        //                .HasForeignKey(s => s.Id);
        //}
    }
}
