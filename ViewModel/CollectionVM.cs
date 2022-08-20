#nullable disable

using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel
{
    public class CollectionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string Owner { get; set; } //?
        public List<ItemVM> Items { get; set; }
        public Dictionary<string, string> AdditionalFieldsInfo { get; set; }

        public CollectionVM() { }
        public CollectionVM(Collection collection)
        {
            Id = collection.Id;
            Name = collection.Name;
            ItemType = collection.ItemType;
            Description = collection.Description;
            IsPrivate = collection.IsPrivate;
            Owner = collection.Owner.UserName;
            AdditionalFieldsInfo = Service.CollectionService.UncompressAdditionalFields(collection.AdditionalFieldsInfo);
            Items = new List<ItemVM>();
            foreach (Item item in collection.Items)
            {
                ItemVM itemVM = new ItemVM(item, this);
                Items.Add(itemVM);
            }
        }
    }
}
