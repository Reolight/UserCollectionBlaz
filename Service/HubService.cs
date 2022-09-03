using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.Service;

public class HubService
{
    public Action<Comment> OnCommentChange;

    public Task SendAsync(Comment comment)
    {
        OnCommentChange.Invoke(comment);
        return Task.FromResult(0);
    }
}