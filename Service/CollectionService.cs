﻿using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.ViewModel;
using System.Text.Json;

namespace UserCollectionBlaz.Service
{
    public class CollectionService
    {
        private readonly AppDbContext _context;
        private static IDbContextFactory<AppDbContext> _factory;
        private readonly UserManager<AppUser> userManager;
        private readonly CloudinaryService cloudinaryService;
        private static List<Tag> Tags;
        private readonly LikeService _likeService;
        public CollectionService(AppDbContext dbContext, CloudinaryService cloudinaryService, UserManager<AppUser> userManager, IDbContextFactory<AppDbContext> factory, LikeService likeService)
        {
            _context = dbContext;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
            _factory = factory;
            _likeService = likeService;
            UpdateTagPool();
            ManageTags();
        }

        private async void UpdateTagPool()
        {
            await using AppDbContext context = await _factory.CreateDbContextAsync();
            Tags = context.Tags
                .Include(tag => tag.Items)
                .ThenInclude(item => item.collection).ToList();
        }
        private async void ManageTags()
        {
            await using AppDbContext context = await _factory.CreateDbContextAsync();
            List<Tag> tagsToDel = new List<Tag>();
            foreach (Tag tag in Tags)
            {
                tag.Items.RemoveAll(item => item.collection is null);
            }
            
            Tags.Where(tag => tag.Items.Count == 0).ToList().ForEach(tag => context.Tags.Remove(tag));
            await context.SaveChangesAsync();
            UpdateTagPool();
        }
        private static async Task<List<CollectionVM>> ConvertCollectionToVM(List<Collection> collections)
        {
            List<CollectionVM> collectionVMs = new();
            if (collections is not null)
            {
                foreach (Collection collection in collections)
                {
                    await using AppDbContext context = await _factory.CreateDbContextAsync();
                    CollectionVM collectionVm = new CollectionVM(collection);
                    collection.Items
                        .ForEach(item =>
                        {
                            Like? like = context.Likes
                                .Include(like => like.LikedBy)
                                .FirstOrDefault(like => like.Position.Contains($"{item.Id}"));
                            collectionVm.Likes += like is null? 0 : like.LikedBy.Count;
                        });

                    collectionVMs.Add(collectionVm);

                }
            }

            return collectionVMs;
        }

        private async Task<List<Collection>> GetAllCollections()
        {
            await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Collections
                .Include(col => col.Items).ThenInclude(item => item.Tags)
                .Include(col => col.Items).ThenInclude(item => item.Likes).ThenInclude(like => like.LikedBy)
                .Include(collection => collection.Owner)
                .ToListAsync();
        }

        private async Task<List<Collection>> GetAllPublicCollections()
        {
            await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
            return await dbContext.Collections.Where(collection => !collection.IsPrivate)
                .Include(collection => collection.Owner)
                .Include(col => col.Items).ThenInclude(item => item.Tags)
                .Include(col => col.Items).ThenInclude(item => item.Likes).ThenInclude(like => like.LikedBy)
                .ToListAsync();
        }

        public async Task<CollectionVM?> GetCollectionVMAsync(int? Id, string? UserName, bool strict = false)
        {
            AppUser? user = string.IsNullOrWhiteSpace(UserName)? null : await userManager.FindByNameAsync(UserName);
            Collection? collection = await (from col in _context.Collections
                                            where col.Id == Id
                                            select col)
                .Include(col => col.Items).ThenInclude(item => item.Tags)
                .Include(col => col.Items).ThenInclude(item => item.Likes).ThenInclude(like => like.LikedBy)
                .Include(collection => collection.Owner)
                .FirstOrDefaultAsync();
            
            if (collection is not null && 
                (strict || collection.IsPrivate) 
                && ((user is null) || (collection.Owner.UserName != user.UserName && !user.IsAdmin))) return null;
            return collection is not null ? new CollectionVM(collection) : null;
        }
        public async Task<List<CollectionVM>?> GetCollectionVMsByAutor(string name, bool isOwner = false)
        {
            List<Collection>? collections = await (from collection in _context.Collections
                                                    where collection.Owner.UserName == name
                                                    select collection)
                .Include(col => col.Items).ThenInclude(item => item.Tags)
                .Include(col => col.Items).ThenInclude(item => item.Likes).ThenInclude(like => like.LikedBy)
                .Include(collection => collection.Owner)
                .ToListAsync();

            return isOwner
                ? await ConvertCollectionToVM(collections)
                : await ConvertCollectionToVM(collections.Where(col => !col.IsPrivate).ToList());
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
            int info = await _context.SaveChangesAsync();
            Console.WriteLine(info);
            return true;
        }
        public async Task<bool> RemoveCollectionAsync(CollectionVM collectionVm)
        {
            Collection? collection = await GetCollection(collectionVm);
            if (collection is null) return false;
            foreach (Item item in collection.Items.ToList())
            {
                foreach (Tag tag in item.Tags)
                    tag.Items.Remove(item);
                await cloudinaryService.DeletePhotoAsync(item.ImageSrc);
                collection.Items.Remove(item);
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditCollectionAsync(CollectionVM collectionVm)
        {
            Collection? collection = await GetCollection(collectionVm);
            if (collection == null) return false;
            collection.Name = collectionVm.Name;
            collection.Description = collectionVm.Description;
            collection.ItemType = collectionVm.ItemType;
            collection.IsPrivate = collectionVm.IsPrivate;
            collection.AdditionalFieldsInfo = CompressAdditionalFields(collectionVm.AdditionalFieldsInfo);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> AddItemToCollectionAsync(ItemVM item)
        {
            Collection? collection = await GetCollection(item.collection);
            if (collection is null) return false;
            Item newItem = new()
            {
                Name = item.Name,
                Description = item.Description,
                collection = collection,
                ImageSrc = item.ImageSrc,
                Tags = item.Tags,
                AdditionalFields = CompressAdditionalFields(item.AdditionalFields)
            };

            _context.AttachRange(newItem.Tags);
            collection.Items.Add(newItem);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> RemoveItemFromCollectionAsync(CollectionVM? collectionVm, ItemVM? delItem)
        {
            await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
            Collection? collection = await GetCollection(collectionVm);
            if (collection is null || delItem is null) return false;
            Item? item = collection.Items.Find(itma => itma.Id == delItem.Id);
            foreach (Tag tag in item.Tags)
                tag.Items.Remove(item);
            if (string.IsNullOrEmpty(item.ImageSrc))
                await cloudinaryService.DeletePhotoAsync(item.ImageSrc);
            collection.Items.Remove(item);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditItemAsync(ItemVM itemVm)
        {
            Collection? collection = await GetCollection(itemVm.collection);
            if (collection is null) return false;
            Item? item = collection.Items.Find(col => col.Id == itemVm.Id);
            if (item is null) return false;
            item.Name = itemVm.Name;
            item.Description = itemVm.Description;
            item.ImageSrc = itemVm.ImageSrc;
            item.Tags = itemVm.Tags;
            item.AdditionalFields = CompressAdditionalFields(itemVm.AdditionalFields);
            await _context.SaveChangesAsync();
            return true;
        }

        public static Dictionary<string, string>? UncompressAdditionalFields(string? compressed)
            => string.IsNullOrEmpty(compressed)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(compressed);
            

        public static string CompressAdditionalFields(Dictionary<string, string> uncompressed)
        {
            if (uncompressed is null || uncompressed.Count == 0) return "";
            return JsonSerializer.Serialize(uncompressed);
        }
        private async Task<Collection?> GetCollection(CollectionVM collectionVM)
        {
            await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
            return (from col in _context.Collections
                where col.Id == collectionVM.Id
                select col).Include(col => col.Items)
                    .ThenInclude(item => item.Tags).FirstOrDefault();
        }

        public async Task<List<string>> GetTagPool()
        {
            return (Tags.Any())
                ? Tags.Select(tag => tag.Name).ToList()
                : new List<string>();
        }

        public async Task<List<CollectionVM>> GetMostLikedCollections(int count, int page = 0)
        => (await ConvertCollectionToVM(
            await GetAllPublicCollections()))
                .OrderByDescending(vm => vm.Likes)
                .Skip(count * page)
                .Take(count).ToList(); 

        public async Task<List<CollectionVM>> GetLargestCollections(int count, int page = 0)
        {
            return await ConvertCollectionToVM(
                (await GetAllPublicCollections())
                    .Where(col => col.Items.Count > 0)
                    .OrderByDescending(col => col.Items.Count)
                    .Skip(count * page)
                    .Take(count).ToList());
        }

        public async Task<Tag> CreateNewTag(string name)
        {
            Tag tag = new Tag() { Name = name };
            Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public Tag? GetTagFromPool(string name) => 
            (from tag in Tags where String.Equals(tag.Name, name, StringComparison.CurrentCultureIgnoreCase) select tag).FirstOrDefault();

        public async Task<List<Tag>> GetAllTags()
            => Tags;

        public async Task<int> GetItemCount()
        {
            int count = 0;
            (await GetAllCollections())
                .ForEach(collection => count += collection.Items.Count);
            return count;
        }
    }
}