using Microsoft.AspNetCore.Identity;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.ViewModel;

namespace UserCollectionBlaz.Service
{
    public class CollectionService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> userManager;
        public CollectionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCollectionAsync(CollectionVM collectionVM)
        {
            Collection collection = new()
            {
                Name = collectionVM.Name,
                Description = collectionVM.Description,
                IsPrivate = collectionVM.IsPrivate,
                Owner = await userManager.FindByNameAsync(collectionVM.Owner),
                AdditionalFieldsInfo = CompressAdditionalFields(collectionVM.AdditionalFieldsInfo)
            };

            List<Item> items = new List<Item>();
            foreach (ItemVM itemVM in collectionVM.Items)
            {
                Item item = new()
                {
                    Name = itemVM.Name,
                    Description = itemVM.Description,
                    ImageSrc = itemVM.ImageSrc,
                    collection = collection,
                    AdditionalFields = CompressAdditionalFields(itemVM.AdditionalFields)
                };
                items.Add(item);
            }

            collection.Items = items;
        }

        public void AddItemToCollection(CollectionVM parentCollection, ItemVM item)
        {

        }

        public Dictionary<string, string> UncompressAdditionalFields(string compressed)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<string> fields = new List<string>(compressed.Split(","));
            foreach (string field in fields)
            {
                string[] KeyValue = field.Split(":");
                result.Add(KeyValue[0], KeyValue[1]);
            }

            return result;
        }

        public string CompressAdditionalFields(Dictionary<string, string> uncompressed)
        {
            List<string> compressed = new List<string>();
            foreach (string key in uncompressed.Keys)
            {
                compressed.Add(key + ":" + uncompressed[key]);
            }

            return string.Join(",", compressed);
        }

    }
}
