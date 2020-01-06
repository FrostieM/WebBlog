using Microsoft.EntityFrameworkCore;

namespace WebBlog.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) 
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Blog)
                .WithOne(b => b.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts)
                .WithOne(p => p.Blog)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithOne(l => l.Post)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
    }
}