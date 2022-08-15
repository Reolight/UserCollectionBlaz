#nullable disable

using System.ComponentModel.DataAnnotations;

namespace UserCollectionBlaz.ViewModel
{
    public class ComVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string PlaceUrl { get; set; }
        [Required]
        public string AutorId { get; set; }
        [Required, MaxLength(300, ErrorMessage = "Your comment must not exceed length of 300 symbols")]
        public string Content { get; set; }
        public DateTime Posted { get; set; }
    }
}
