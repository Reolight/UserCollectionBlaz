#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserCollectionBlaz.ViewModel;

namespace UserCollectionBlaz.Areas.Identity.Data;

public class AppUser : IdentityUser
{
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime BannedSince { get; set; }
    public TimeSpan BanLasts { get; set; }
    public string AvatarSrc { get; set; }
    public int PostedTimes { get; set; }
    public int Level { get; set; } = 1;
    public ICollection<Comment> Comments { get; set; }
    public List<Collection> Collections { get; set; }
     
}