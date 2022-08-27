using System.ComponentModel.DataAnnotations;

namespace UserCollectionBlaz.Areas.Identity.Data;

public class Tag
{
    [Key]
    [Required]
    public string Name { get; set; }
    public List<Item> Items { get; set; }
}