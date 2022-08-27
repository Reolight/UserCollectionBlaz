#nullable disable

using System.ComponentModel.DataAnnotations;
using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel
{
    public class ItemVM
    {
        public int Id { get; set; }
        [Required, MinLength(3)]
        public string Name { get; set; }
        [MaxLength(400)]
        public string Description { get; set; }
        public string ImageSrc { get; set; }
        public CollectionVM collection { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
        public List<Tag> Tags { get; set; }
        public ItemVM() { }

        /// <summary>
        /// Constructor of an item view model with mapped empty additional fields according to established rules of the collection
        /// </summary>
        /// <param name="collection">is a collection view model wich determines rules for additional fields of the item.
        /// Besides the item will be declared as the item of this collection</param>
        public ItemVM(CollectionVM collection)
        {
            this.collection = collection;
            AdditionalFields = new Dictionary<string, string>();
            foreach (string fieldName in collection.AdditionalFieldsInfo.Keys)
                AdditionalFields.Add(fieldName, "");
            Tags = new List<Tag>();
        }
        public ItemVM(Item item, CollectionVM collectionVM)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            ImageSrc = item.ImageSrc;
            collection = collectionVM;
            AdditionalFields = Service.CollectionService.UncompressAdditionalFields(item.AdditionalFields);
            Tags = item.Tags ?? new List<Tag>();
        }
    }
}
