using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.ViewModel;

namespace UserCollectionBlaz.Service
{
    public class UserService
    {
        private readonly AppDbContext dbContext;
        private readonly UserManager<AppUser> userManager;

        public UserService(AppDbContext context, UserManager<AppUser> manager)
        {
            dbContext = context;
            userManager = manager;
        }

        public AppUser? GetHeavyUser(string name)
        {
            var userData = (from user in dbContext.Users
                            where user.UserName == name
                            select user).Include(u => u.Collections).FirstOrDefault();
            return userData;
        }

        /// <summary>
        /// Use to get user view model. It is shorted version of AppUser
        /// </summary>
        /// <param name="name"></param>
        /// <returns>UserVM class by name</returns>
        /// 
        public async Task<UserVM?> GetVM(string name)
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

        private bool IsLevelRequirementsAchieved(AppUser user)
        {
            if (user.PostedTimes == UserVM.GetMaxPostForNewLevel(user.Level))
            {
                user.Level++;
                dbContext.SaveChanges();
                return true;
            }
            
            return false;
        }

        public async Task<bool> HavePostedAnotherOne(UserVM user) 
            => await HavePostedAnotherOne(await userManager.FindByNameAsync(user.UserName));
        public async Task<bool> HavePostedAnotherOne(AppUser user)
        {
            if (user.Level == 0) user.Level = 1;
            user.PostedTimes++;
            return IsLevelRequirementsAchieved(user);
        }
        
        public async Task<IdentityResult> Update(UserVM userVM)
        {
            AppUser user = await userManager.FindByNameAsync(userVM.UserName);
            user.UserName = userVM.UserName;
            user.Email = userVM.Email;
            user.AvatarSrc = userVM.AvatarSrc;
            return await userManager.UpdateAsync(user);
        }

        public async void UpdateLevel(string name, int level)
        {
            AppUser user = await userManager.FindByNameAsync(name);
            user.Level = level;
            user.PostedTimes = UserVM.GetMinPostForThisLevel(level);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> AdminPowerChange(string maybeLuckyUser)
        {
            AppUser user = await userManager.FindByNameAsync(maybeLuckyUser);
            if (user == null) return false;
            user.IsAdmin = !user.IsAdmin;
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> BanHummer(string unluckyUser) 
            => await BanHummerAsync(unluckyUser, new TimeSpan(0, 2, 28));
        public async Task<bool> BanHummerAsync(string unluckyUser, TimeSpan banLasts)
        {
            AppUser user = await userManager.FindByNameAsync(unluckyUser);
            if (user == null) return false;
            user.BannedSince = DateTime.Now;
            user.BanLasts = banLasts;
            user.IsBlocked = true;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
