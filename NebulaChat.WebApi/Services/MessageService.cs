using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NebulaChat.Core.Dto;
using NebulaChat.Core.Models;
using NebulaChat.Data;
using NebulaChat.WebApi.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NebulaChat.WebApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly NebulaChatDbContext dbContext;
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IMapper mapper;
        private readonly IMessageRepository messageRepository;
        public MessageService(NebulaChatDbContext dbContext, IHubContext<ChatHub> hubContext, IMapper mapper, IMessageRepository messageRepository)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
            this.mapper = mapper;
            this.messageRepository = messageRepository;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await messageRepository.GetMessagesAsync();
        }
        public IEnumerable<Message> GetMessages()
        {
            return messageRepository.GetMessages();
        }
        public async Task<Message> GetMessage(Guid messageId)
        {
            return await messageRepository.GetMessage(messageId);
        }
        public async Task CreateMessageAsync(Message message)
        {
            message.Id = Guid.NewGuid();
            await dbContext.Messages.AddAsync(message);
            var success = await dbContext.SaveChangesAsync() > 0;
            
            if (success)
            {
                var messageNotification = mapper.Map<SendMessageNotification>(await GetMessage(message.Id));
                await hubContext.Clients.All.SendAsync("BroadcastMessage", messageNotification);
            }

        }
        public void PurgeMessages(int daysAfterMessagePosted)
        {
            messageRepository.Purge(daysAfterMessagePosted);
        }

        public async Task<IEnumerable<TopTenResponse>> GetTopTenChatters()
        {
            return await messageRepository.TopTen();
        }
    }
}
