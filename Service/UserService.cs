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
        private readonly SignInManager<AppUser> signInManager;
        public UserService(AppDbContext appDb, UserManager<AppUser> manager, SignInManager<AppUser> signInManager)
        {
            dbContext = appDb;
            userManager = manager;
            this.signInManager = signInManager;
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

            await dbContext.SaveChangesAsync();
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

        public async Task<bool> AdminPowerChange(string maybeLuckyUser)
        {
            AppUser user = await userManager.FindByNameAsync(maybeLuckyUser);
            if (user == null) return false;
            user.IsAdmin = !user.IsAdmin;
            return true;
        }
        public async Task<bool> BanHummer(string unluckyUser) => await BanHummerAsync(unluckyUser, new TimeSpan(0, 2, 28));

        public async Task<bool> BanHummerAsync(string unluckyUser, TimeSpan banLasts)
        {
            AppUser user = await userManager.FindByNameAsync(unluckyUser);
            if (user == null) return false;
            user.BannedSince = DateTime.Now;
            user.BanLasts = banLasts;
            user.IsBlocked = true;
            return true;
        }
    }
}
