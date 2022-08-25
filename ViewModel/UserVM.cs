#nullable disable

using System.ComponentModel.DataAnnotations;
using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel
{
    public class UserVM : ICloneable
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Level { get; set; }
        public int PostedTimes { get; set; }
        public string AvatarSrc { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }

        public static int GetMaxPostForNewLevel(int level)
        {
            int MaxPostForNewLevel = 0;
            for (int i = 1; i <= level; i++) MaxPostForNewLevel += (int)(10 * Math.Pow(1.2, i - 1));
            return MaxPostForNewLevel;
        }

        public static int GetMinPostForThisLevel(int level)
        {
            int MinPostForNewLevel = 0;
            for (int i = 2; i <= level; i++) MinPostForNewLevel += (int)(10 * Math.Pow(1.2, i - 2));
            return MinPostForNewLevel;
        }
        public UserVM() { }
        public UserVM(AppUser user)
        {
            UserName = user.UserName;
            Email = user.Email;
            Level = user.Level;
            PostedTimes = user.PostedTimes;
            AvatarSrc = user.AvatarSrc;
            IsAdmin = user.IsAdmin;
            IsBlocked = user.IsBlocked;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
