using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.ViewModel;

namespace UserCollectionBlaz.Service
{
    public class CollectionService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly CloudinaryService cloudinaryService;
        public CollectionService(AppDbContext context, CloudinaryService cloudinaryService, UserManager<AppUser> userManager)
        {
            _context = context;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
        }

        private static List<CollectionVM> ConvertCollectionToVM(List<Collection> collections)
        {
            List<CollectionVM> collectionVMs = new();
            if (collections is not null)
            {
                foreach (Collection collection in collections)
                {
                    collectionVMs.Add(new CollectionVM(collection));
                }
            }

            return collectionVMs;
        }
        public async Task<CollectionVM?> GetCollectionVMAsync(int? Id)
        {
            Collection? collection = await (from col in _context.Collections
                                            where col.Id == Id
                                            select col).Include(col => col.Items).FirstOrDefaultAsync();
            if (collection is not null)
                return new CollectionVM(collection);
            else
                return null;
        }
        public async Task<List<CollectionVM>?> GetCollectionVMsByAutor(string name)
        {
            List<Collection>? collections = (from collection in _context.Collections
                                            where collection.Owner.UserName == name 
                                            select collection).ToList();
            return ConvertCollectionToVM(collections);
        }
        public async Task<List<CollectionVM>?> GetAllCollectionVMsAsync()
        {
            return await new Task<List<CollectionVM>>(() =>
            {
                List<Collection> collections = _context.Collections.Include(col => col.Items).ToList();
                return ConvertCollectionToVM(collections);
            });
        }
        public async Task<bool> AddCollectionAsync(CollectionVM collectionVM)
        {
            Collection collection = new()
            {
                Name = collectionVM.Name,
                Description = collectionVM.Description,
                ItemType = collectionVM.ItemType,
                IsPrivate = collectionVM.IsPrivate,
                Owner = await userManager.FindByNameAsync(collectionVM.Owner),
                Items = new List<Item>(),
                AdditionalFieldsInfo = CompressAdditionalFields(collectionVM.AdditionalFieldsInfo)
            };

            _context.Collections.Add(collection);
            int info = _context.SaveChanges();
            Console.WriteLine(info);
            return true;
        }
        public async Task<bool> RemoveCollectionAsync(CollectionVM collectionVM)
        {
            Collection? collection = GetCollection(collectionVM);
            if (collection is null) return false;
            foreach (Item item in collection.Items)
            {
                await cloudinaryService.DeletePhotoAsync(item.ImageSrc);
                collection.Items.Remove(item);
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> TogglePrivateMode(CollectionVM collectionVM)
        {
            Collection? collection = GetCollection(collectionVM);
            if (collection is null) return false;
            collection.IsPrivate = !collection.IsPrivate;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditCollectionAsync(CollectionVM collectionVM)
        {
            Collection? collection = GetCollection(collectionVM);
            if (collection == null) return false;
            collection.Name = collectionVM.Name;
            collection.Description = collectionVM.Description;
            collection.ItemType = collectionVM.ItemType;
            collection.IsPrivate = collectionVM.IsPrivate;
            collection.AdditionalFieldsInfo = CompressAdditionalFields(collectionVM.AdditionalFieldsInfo);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddItemToCollectionAsync(CollectionVM collectionVM, ItemVM item)
        {
            Collection? collection = GetCollection(collectionVM);
            if (collection is null) return false;
            collection.Items.Add(new Item
            {
                Name = item.Name,
                Description = item.Description,
                collection = collection,
                ImageSrc = item.ImageSrc,
                AdditionalFields = CompressAdditionalFields(item.AdditionalFields)
            });
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveItemFromCollectionAsync(CollectionVM collectionVM, ItemVM delItem)
        {
            Collection? collection = GetCollection(collectionVM);
            if (collection is null) return false;
            Item? item = collection.Items.Find(itma => itma.Id == delItem.Id);
            await cloudinaryService.DeletePhotoAsync(item.ImageSrc);
            collection.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditItemAsync(CollectionVM collectionVM, ItemVM itemVM)
        {
            Collection? collection = GetCollection(collectionVM);
            if (collection is null) return false;
            Item? item = collection.Items.Find(col => col.Id == itemVM.Id);
            if (item is null) return false;
            item.Name = itemVM.Name;
            item.Description = itemVM.Description;
            item.ImageSrc = itemVM.ImageSrc;
            item.AdditionalFields = CompressAdditionalFields(itemVM.AdditionalFields);
            await _context.SaveChangesAsync();
            return true;
        }
        public static Dictionary<string, string> UncompressAdditionalFields(string? compressed)
        {
            if (string.IsNullOrEmpty(compressed)) return new();
            Dictionary<string, string> result = new();
            List<string> fields = new(compressed.Split(","));
            foreach (string field in fields)
            {
                string[] KeyValue = field.Split(":");
                result.Add(KeyValue[0], KeyValue[1]);
            }

            return result;
        }
        public static string CompressAdditionalFields(Dictionary<string, string> uncompressed)
        {
            List<string> compressed = new();
            foreach (string key in uncompressed.Keys)
            {
                compressed.Add(key + ":" + uncompressed[key]);
            }

            return string.Join(",", compressed);
        }
        private Collection? GetCollection(CollectionVM collectionVM) => (from col in _context.Collections
                                                                        where col.Id == collectionVM.Id
                                                                        select col).Include(col => col.Items).FirstOrDefault();
    }
}