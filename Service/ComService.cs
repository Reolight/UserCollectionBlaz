using UserCollectionBlaz.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UserCollectionBlaz.Service
{
    /// <summary>
    /// Service for maintain commentaries: saving and 
    /// </summary>
    public class ComService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDbContextFactory<AppDbContext> _factory;
        private readonly HubService _hubService;
        
        public ComService(AppDbContext dbContext, UserManager<AppUser> userManager, IDbContextFactory<AppDbContext> factory, HubService hubService)
        {
            _userManager = userManager;
            _factory = factory;
            _hubService = hubService;
        }

        public async Task Add(Comment item)
        {
            await using AppDbContext db = await _factory.CreateDbContextAsync();
            db.Users.Attach(item.Autor);
            db.Comments.Add(item);
            await db.SaveChangesAsync();
            await _hubService.SendAsync(item);
        }

        /// <summary>
        /// This method is design for retrieving comments which are belong to particular Url
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>?> GetAllByKey(string key)
        {
            await using AppDbContext db = await _factory.CreateDbContextAsync();
            return await db.Comments
                .Where(com => com.PlaceUrl == key)
                .Include(com => com.Autor)
                .Select(com => com).ToListAsync();
        }
        
        
    }
}
