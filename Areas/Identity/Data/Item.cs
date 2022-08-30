#nullable disable

namespace UserCollectionBlaz.Areas.Identity.Data
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
        public Collection collection { get; set; }
        public string AdditionalFields { get; set; }
        public List<Tag> Tags { get; set; }
        public Like Likes { get; set; }
    }
}
