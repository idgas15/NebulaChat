using Microsoft.AspNetCore.SignalR;
using NebulaChat.WebApi.Hubs;
using System.Threading.Tasks;

namespace NebulaChat.Services
{
    public class NotifyService : INotifyService
    {
        private readonly IHubContext<ChatHub> hub;

        public NotifyService(IHubContext<ChatHub> hub)
        {
            this.hub = hub;
        }

        public Task SendNotificationAsync(string user, string message)
        {
            return hub.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

    public interface INotifyService
    {
        Task SendNotificationAsync(string user, string message);
    }
}
