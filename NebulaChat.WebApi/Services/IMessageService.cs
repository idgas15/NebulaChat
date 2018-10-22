using NebulaChat.Core.Dto;
using NebulaChat.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NebulaChat.WebApi.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        IEnumerable<Message> GetMessages();
        Task CreateMessageAsync(Message message);
        Task<Message> GetMessage(Guid messageId);
        void PurgeMessages(int daysAfterMessagePosted);
        Task<IEnumerable<TopTenResponse>> GetTopTenChatters();
    }
}
