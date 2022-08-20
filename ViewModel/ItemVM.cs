#nullable disable

using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel
{
    public class ItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
        public CollectionVM collection { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }

        public ItemVM() { }
        public ItemVM(Item item, CollectionVM collectionVM)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            ImageSrc = item.ImageSrc;
            collection = collectionVM;
        }
    }
}
