#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserCollectionBlaz.Areas.Identity.Data
{
    public class Comment
    {
        [Required]
        public int Id { get; set; }
        public string PlaceUrl { get; set; }
        [ForeignKey("AppUser")]
        public AppUser Autor { get; set; }
        public string Content { get; set; }
        public DateTime PostedTime { get; set; }
    }
}
