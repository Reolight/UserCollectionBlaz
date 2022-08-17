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
        public CommentsHub(ComService comService, UserService userService)
        {
            _comService = comService;
            _userService = userService;
        }

        public async Task PostCom(Comment com)
        {
            await _userService.HavePostedAnotherOne(com.AutorName);
            _comService.Add(com);
            await Clients.All.SendAsync("RecieveCom", com);
        }
    }
}
