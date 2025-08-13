using System.Reflection;
using Day06.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Day06.API.Context.cs
{
    public class BlogPostContext:DbContext
    {
        public BlogPostContext()
        {
            
        }


        public BlogPostContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<BlogPostEntity> BlogPosts { get; set; }
        public DbSet<Outline> Outlines { get; set; }
        public DbSet<ContentBlock> ContentBlocks { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = "Server=YUSUF\\YUSUF;Database=SemanticKernal;User Id=sa;Password=password1;TrustServerCertificate=True; Encrypt=false";
            optionsBuilder.UseSqlServer(connStr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // BlogPost → Outline (One-to-Many)
            modelBuilder.Entity<Outline>()
                .HasOne(o => o.BlogPost)
                .WithMany(b => b.Outlines)
                .HasForeignKey(o => o.BlogPostId);

            // Outline → ContentBlock (One-to-Many)
            modelBuilder.Entity<ContentBlock>()
                .HasOne(c => c.Outline)
                .WithMany(o => o.ContentBlocks)
                .HasForeignKey(c => c.OutlineId);

            // ContentBlock → Review (One-to-Many)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.ContentBlock)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.ContentBlockId);
        }

    }
}
