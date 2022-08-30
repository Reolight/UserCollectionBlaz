using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.ViewModel;

namespace UserCollectionBlaz.Service;

public class LikeService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IDbContextFactory<AppDbContext> _factory;

    public LikeService(UserManager<AppUser> userManager, IDbContextFactory<AppDbContext> factory)
    {
        _userManager = userManager;
        _factory = factory;
    }

    public async Task<LikeVM> Update(LikeVM component, string userName)
    {
        await using AppDbContext context = await _factory.CreateDbContextAsync();
        Like like = await context.Likes
            .Where(like1 => like1.Position == component.Position)
            .Include(like => like.LikedBy)
            .FirstAsync();
        AppUser user = context.Users.First(user => user.UserName == userName);
        if (like.LikedBy.Contains(user))
            like.LikedBy.Remove(user);
        else
            like.LikedBy.Add(user);
        //context.Entry(like.LikedBy).State = EntityState.Unchanged;
        await context.SaveChangesAsync();
        return new LikeVM(like);
    }

    /// <summary>
    /// Creates new Like model and saves it in database
    /// </summary>
    /// <param name="location">is a string with pattern "item/comment-{id}"</param>
    /// <returns>Like visual model for like component</returns>
    private async Task<Like> CreateNew(string location)
    {
        await using AppDbContext context = await _factory.CreateDbContextAsync();
        Like likedComponent = new()
        {
            Position = location,
            LikedBy = new List<AppUser>()
        };

        context.Likes.Add(likedComponent);
        await context.SaveChangesAsync();
        return likedComponent;
    }
    
    /// <summary>
    /// Returns exact like model by Uri and Id of the component
    /// </summary>
    /// <param name="location">is a string with pattern "item/comment-{id}"</param>
    /// <returns>Exact Like model</returns>
    public async Task<LikeVM> GetByLocation(string location)
    {
        await using AppDbContext context = await _factory.CreateDbContextAsync();
        var lk = new LikeVM(await (from like in context.Likes
                where like.Position == location
                select like).Include(like => like.LikedBy)
            .FirstOrDefaultAsync() ?? await CreateNew(location));
        return lk;
    }

    public async Task<int> GetLikesCount(string location)
    {
        await using AppDbContext context = await _factory.CreateDbContextAsync();
        return (await context.Likes
            .Where(like => like.Position == location)
            .Include(like => like.LikedBy)
            .FirstAsync()).LikedBy.Count;
    }
}