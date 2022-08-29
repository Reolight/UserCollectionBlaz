using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserCollectionBlaz.Areas.Identity.Data;

public class Like
{
    [Required, Key]
    public string Position { get; set; }
    [ForeignKey("AppUser")]
    public List<AppUser> LikedBy { get; set; }
}