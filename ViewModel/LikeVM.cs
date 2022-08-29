using UserCollectionBlaz.Areas.Identity.Data;

namespace UserCollectionBlaz.ViewModel;

public class LikeVM
{
    public string Position { get; set; }
    public List<string> LikedBy { get; set; }

    public LikeVM() { }

    public LikeVM(Like like)
    {
        Position = like.Position;
        LikedBy = like.LikedBy.Select(user => user.UserName).ToList();
    }
}