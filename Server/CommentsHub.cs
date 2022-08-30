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
        
        public CommentsHub(ComService comService)
        {
            _comService = comService;
        }
        public async Task PostCom(Comment com)
        {
            await _comService.Add(com);
            await Clients.All.SendAsync("RecieveCom", com);
        }
    }
}
