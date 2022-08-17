using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.ViewModel;

namespace UserCollectionBlaz.Service
{
    public class UserService
    {
        private readonly AppDbContext dbContext;
        private readonly UserManager<AppUser> userManager;
        public UserService(AppDbContext appDb, UserManager<AppUser> manager)
        {
            dbContext = appDb;
            userManager = manager;
        }

        //[HttpGet("profile-get/{name}")]
        public async Task<UserVM> Get(string name)
        {
            var uvm = new UserVM((from user in dbContext.Users 
                           where user.UserName == name 
                           select user).First<AppUser>());
            return uvm;     
        }

        public IEnumerable<AppUser>? GetAll()
        {
            return userManager.Users.ToList();
        }

        public async Task<bool> HavePostedAnotherOne(string userName)
        {
            AppUser user = await userManager.FindByNameAsync(userName);
            if (user.Level == 0) user.Level = 1;
            bool lvled = false;
            user.PostedTimes++;
            if (user.PostedTimes == (int)(10 * Math.Pow(1.2, user.Level)))
            {
                user.Level++;
                lvled = true;
            }

            dbContext.SaveChanges();
            return lvled;
        }
    }
}
