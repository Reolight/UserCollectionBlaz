#nullable disable

namespace UserCollectionBlaz.Areas.Identity.Data
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public AppUser Owner { get; set; }
        public List<Item> Items { get; set; }
        public string AdditionalFieldsInfo { get; set; }
    }
}
