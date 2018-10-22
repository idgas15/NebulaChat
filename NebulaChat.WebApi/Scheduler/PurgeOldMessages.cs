using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NebulaChat.Core.Dto;
using NebulaChat.WebApi.Hubs;
using NebulaChat.WebApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NebulaChat.WebApi.Scheduler
{
    public class PurgeOldMessages : ScheduledProcessor
    {
        public PurgeOldMessages(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            
        }
        private const int purgeAfterDays = 5;
        protected override string Schedule => "30 * * * *";

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            try
            {
                var messageService = serviceProvider.GetService<IMessageService>();
                System.Diagnostics.Trace.WriteLine($"Purging old messages {DateTime.Now}");
                messageService.PurgeMessages(purgeAfterDays);

                var hubContext = serviceProvider.GetService<IHubContext<ChatHub>>();
                var mapper = serviceProvider.GetService<IMapper>();

                var messages = messageService.GetMessages().Select(c => new SendMessageNotification
                {
                    Author = new ChatUserDto
                    {
                        FirstName = c.Author.FirstName,
                        LastName = c.Author.LastName,
                        Id = c.AuthorId,
                        Username = c.Author.Username
                    },
                    Recipient = new ChatUserDto
                    {
                        FirstName = c.Recipient.FirstName,
                        LastName = c.Author.LastName,
                        Id = c.RecipientId,
                        Username = c.Author.Username
                    },
                    Content = c.Content,
                    CreatedDate = c.CreatedDate
                });
                // Automapper not playing nice!
                //var mappedMessages = mapper.Map<SendMessageNotification>(messages);

                hubContext.Clients.All.SendAsync("PostedMessagesRefresh", messages);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
