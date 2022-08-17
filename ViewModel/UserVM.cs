#nullable disable

using System.ComponentModel.DataAnnotations;
using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel
{
    public class UserVM
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Level { get; set; }
        public int PostedTimes { get; set; }
        public string AvatarSrc { get; set; }

        public UserVM(AppUser user)
        {
            UserName = user.UserName;
            Email = user.Email;
            Level = user.Level;
            PostedTimes = user.PostedTimes;
            AvatarSrc = user.AvatarSrc;
        }
    }
}
