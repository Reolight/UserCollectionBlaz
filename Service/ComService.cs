using UserCollectionBlaz.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore; 

namespace UserCollectionBlaz.Service
{
    /// <summary>
    /// Service for maintain commentaries: saving and 
    /// </summary>
    public class ComService : IService<Comment, int>
    {
        private readonly AppDbContext _context;
        public ComService(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Comment item)
        {
            _context.comments.Add(item);
            _context.SaveChanges();
        }

        public Comment? Get(int id)
        {
            return (from a in _context.comments
                    where a.Id == id 
                    orderby a.PostedTime descending 
                    select a).First();
        }

        public IEnumerable<Comment>? GetAll()
        {
            return _context.comments.ToList();
        }


        /// <summary>
        /// This method is design for retrieving comments which are belong to particular Url
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<Comment>? GetAllByKey(string key)
        {
            return _context.comments
                .Where(com => com.PlaceUrl == key)
                .Include(com => com.Autor)
                .Select(com => com);
        }

        public void Remove(Comment item)
        {
            _context.comments.Remove(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// Doesn't works for comments for now
        /// </summary>
        /// <returns>NotImplementedException</returns>
        public void Set(int key, Comment value)
        {
            throw new NotImplementedException("It doesn't implemented for Comments yet");
        }
    }
}
