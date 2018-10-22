using Microsoft.AspNetCore.SignalR;
using NebulaChat.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NebulaChat.WebApi.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        public Task Subscribe(string chatroom)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, chatroom);
        }

        public Task Unsubscribe(string chatroom)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatroom);
        }
        public async Task SendMessage(SendMessageNotification messageNotification)
        {
            await Clients.All.BroadcastMessage(messageNotification);
        }
        public async Task PostedMessageRefresh(IEnumerable<SendMessageNotification> messages)
        {
            await Clients.All.PostedMessagesRefresh(messages);
        }
        public async Task UsersRefresh(IEnumerable<ChatUserDto> users)
        {
            await Clients.All.UsersRefresh(users);
        }
    }
    public interface IChatHub
    {
        Task SetConnectionId(string connectionId);
        Task BroadcastMessage(SendMessageNotification messageNotification);
        Task PostedMessagesRefresh(IEnumerable<SendMessageNotification> messages);
        Task UsersRefresh(IEnumerable<ChatUserDto> users);
    }
}
