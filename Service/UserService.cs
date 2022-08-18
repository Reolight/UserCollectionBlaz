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

        /// <summary>
        /// Use to get user view model. It is shorted version of AppUser
        /// </summary>
        /// <param name="name"></param>
        /// <returns>UserVM class by name</returns>
        public async Task<UserVM?> Get(string name)
        {
            var userData = (from user in dbContext.Users
                       where user.UserName == name
                       select user).FirstOrDefault();
            return userData is null? null : new UserVM(userData);     
        }

        public IEnumerable<AppUser>? GetAll()
        {
            return userManager.Users.ToList();
        }

        public async Task<bool> HavePostedAnotherOne(AppUser user)
        {
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

        public async Task<IdentityResult> Update(UserVM userVM)
        {
            AppUser user = await userManager.FindByNameAsync(userVM.UserName);
            user.UserName = userVM.UserName;
            user.Email = userVM.Email;
            user.AvatarSrc = userVM.AvatarSrc;
            return await userManager.UpdateAsync(user);
        }
    }
}
