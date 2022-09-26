using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserCollectionBlaz.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Tag> Tags { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //builder.Entity<Comment>()
        //    .HasOne(comment => comment.Autor)
        //    .WithMany(user => user.Comments);
        //builder.Entity<Item>()
        //    .HasOne(item => item.collection)
        //    .WithMany(col => col.Items);
        //builder.Entity<AppUser>()
        //    .HasMany(user => user.Collections);
        //builder.Entity<Item>()
        //    .HasMany(item => item.Tags)
        //    .WithMany(item => item.Items);
        //builder.Entity<Like>()
        //    .HasMany(like => like.LikedBy)
        //    .WithMany(user => user.Liked);
        //builder.Entity<Item>()
        //    .HasOne(item => item.Likes);
    }
}
