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
        public string AutorName { get; set; }
        public string Content { get; set; }
        public DateTime PostedTime { get; set; }
    }
}
