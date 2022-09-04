using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Radzen;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.ViewModel;
using Collection = UserCollectionBlaz.Pages.Collections.Collection;

namespace UserCollectionBlaz.Service;

public class SearchService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public SearchService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }
    
    private async Task<List<SearchResultVM>> SearchForTaggedItems(string query)
    {
        await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
        List<SearchResultVM> resultVms = new List<SearchResultVM>();
        var q = query.Split().ToList();
        dbContext.Tags.Include(tag => tag.Items)
            .ThenInclude(item => item.collection).ThenInclude(collection => collection.Owner)
            .Where(tag => q.Contains(tag.Name))
            .ToList()
                .ForEach(tag => tag.Items
                    .ForEach(item => resultVms.Add(new SearchResultVM(item, query))));
        return resultVms;
    }
    
    private string QueryCleaner(string dirty)
    {
        "!@$%^&*()_+-=/<>,.?\\\"'|`~;:[{}]".ToCharArray().ToList()
            .ForEach(c =>dirty = dirty.Replace($"{c}", ""));
        return dirty;
    }

    private async Task<List<SearchResultVM>?> SearchForItems(string query)
    {
        await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
        List<SearchResultVM> resultVms = new();
        dbContext.Collections
            .Include(col => col.Items).ThenInclude(item => item.Tags)
            .Include(col => col.Owner).ToList()
            .ForEach(collection => collection.Items
                .ForEach(item => resultVms.Add(new SearchResultVM(item, query))));
        return resultVms.Where(vm => vm.Score > 0).ToList();
    }

    private async Task<List<SearchResultVM>?> SearchForCollections(string query)
    {
        await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
        List<SearchResultVM> resultVms = new();
        dbContext.Collections
            .Include(col => col.Owner).ToList()
                .ForEach(collection => resultVms.Add(new SearchResultVM(collection, query)));
        return resultVms.Where(vm => vm.Score > 0).ToList();
    }

    private async Task<List<SearchResultVM>?> SearchForComments(string query)
    {
        await using AppDbContext dbContext = await _factory.CreateDbContextAsync();
        List<SearchResultVM> resultVms = new();
        dbContext.Comments.Include(com => com.Autor).ToList()
            .ForEach(com => resultVms.Add(new SearchResultVM(com, query)));
        return resultVms.Where(vm => vm.Score > 0).ToList();
    }

    public async Task<List<SearchResultVM>?> SearchFor(string query, bool items, bool collections, bool comments)
    {
        
        query = QueryCleaner(query);
        if (query.StartsWith('#'))
            return await SearchForTaggedItems(query.Replace("#", ""));
        List<SearchResultVM> resultVms = new();
        if (items)
            resultVms.AddRange(await SearchForItems(query) ?? new ());
        if (collections)
            resultVms.AddRange(await SearchForCollections(query) ?? new ());
        if (comments)
            resultVms.AddRange(await SearchForComments(query) ?? new());
        return resultVms.OrderByDescending(vm => vm.Score).ToList();
    }
}