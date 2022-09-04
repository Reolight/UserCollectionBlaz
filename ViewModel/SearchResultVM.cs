using System.Globalization;
using System.Text.RegularExpressions;
using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel;

public class SearchResultVM
{
    public int Score { get; set; }
    public string? ImageSrc { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string? DateCreated { get; set; }
    public string PostedAt { get; set; }
    public string AutorName { get; set; }
    public List<string>? tags { get; set; }

    private string MarkdownSearched(string source, string query, bool doMarkDown = true, int factor = 1)
    {
        Score += Regex.Matches(source.ToLower(), query.ToLower()).Count * factor;
        return (query.Length > 3 && doMarkDown) ? 
            source.Replace(query, $"<span style=\"color:MediumSeaGreen;\">{query}</span>", true, CultureInfo.InvariantCulture)
            : source;
    }

    public SearchResultVM(Comment comment, string query)
    {
        Content = comment.Content;
        query.Split(' ').ToList().ForEach(q => Content = MarkdownSearched(Content, q));
        PostedAt = comment.PlaceUrl;
        DateCreated = comment.PostedTime.ToString("g");
        AutorName = comment.Autor.UserName;
        Name = AutorName;
        ImageSrc = comment.Autor.AvatarSrc;
    }

    public SearchResultVM(Item item, string query)
    {
        var querySplinted = query.Split(' ').ToList(); 
        Content = item.Description;
        querySplinted.ForEach(q => Content = MarkdownSearched(Content, q));
        Name = item.Name;
        querySplinted.ForEach(q => MarkdownSearched(Name, q, false, 10));
        ImageSrc = item.ImageSrc;
        PostedAt = $"/item/{item.collection.Id}/{item.Id}";
        AutorName = item.collection.Owner.UserName;
        tags = item.Tags.Select(tag => tag.Name).ToList();
        tags.ForEach(tag => 
            querySplinted.ForEach(
                q => MarkdownSearched(tag, q, false, 50)));
    }

    public SearchResultVM(Collection collection, string query)
    {
        var querySplinted = query.Split(' ').ToList(); 
        Content = collection.Description;
        querySplinted.ForEach(q => Content = MarkdownSearched(Content, q));
        Name = collection.Name;
        querySplinted.ForEach(q => MarkdownSearched(Name, q, false, 10));
        PostedAt = $"/collection/{collection.Id}";
        AutorName = collection.Owner.UserName;
    }
}