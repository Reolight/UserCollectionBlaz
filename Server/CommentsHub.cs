using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using UserCollectionBlaz.Areas.Identity.Data;
using UserCollectionBlaz.Service;
using UserCollectionBlaz.ViewModel;

namespace UserCollectionBlaz.Server
{
    public class CommentsHub : Hub
    {
        private readonly ComService _comService;
        private readonly UserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public CommentsHub(ComService comService, UserService userService, UserManager<AppUser> userManager)
        {
            _comService = comService;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task PostCom(ComVM comment)
        {
            Comment com = new Comment()
            {
                Content = comment.Content,
                Autor = await _userManager.FindByNameAsync(comment.AutorId),
                PlaceUrl = comment.PlaceUrl,
                PostedTime = comment.Posted
            };

            _comService.Add(com);
            await Clients.All.SendAsync("RecieveCom", com);
        }
    }
}
