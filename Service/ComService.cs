using UserCollectionBlaz.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using UserCollectionBlaz.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace UserCollectionBlaz.Service
{
    /// <summary>
    /// Service for maintain commentaries: saving and 
    /// </summary>
    public class ComService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ComService(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }

        public async void Add(ComVM comVM)
        {
            Add(new Comment()
            {
                Content = comVM.Content,
                Autor = await _userManager.FindByNameAsync(comVM.AutorId),
                PlaceUrl = comVM.PlaceUrl,
                PostedTime = comVM.Posted
            });
        }
        public async Task Add(Comment item)
        {
            _context.Comments.Add(item);
            await _context.SaveChangesAsync();
        }

        public Comment? Get(int id)
        {
            return (from a in _context.Comments
                    where a.Id == id 
                    orderby a.PostedTime descending 
                    select a).Include(com => com.Autor).First();
        }

        public IEnumerable<Comment>? GetAll()
        {
            return _context.Comments.Include(com => com.Autor).ToList();
        }


        /// <summary>
        /// This method is design for retrieving comments which are belong to particular Url
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<Comment>? GetAllByKey(string key)
        {
            return _context.Comments
                .Where(com => com.PlaceUrl == key)
                .Include(com => com.Autor)
                .Select(com => com);
        }

        public async Task Remove(Comment item)
        {
            _context.Comments.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
