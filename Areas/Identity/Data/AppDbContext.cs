using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserCollectionBlaz.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Comment> comments { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Comment>()
            .HasOne(comment => comment.Autor)
            .WithMany(user => user.Comments);
        builder.Entity<Item>()
            .HasOne(item => item.collection)
            .WithMany(col => col.Items);
    }
}
