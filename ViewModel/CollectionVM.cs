#nullable disable

using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel
{
    public class CollectionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string Owner { get; set; } //?
        public List<ItemVM> Items { get; set; }
        public Dictionary<string, string> AdditionalFieldsInfo { get; set; }

    }
}
