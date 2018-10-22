using Microsoft.EntityFrameworkCore;
using NebulaChat.Core.Dto;
using NebulaChat.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NebulaChat.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly NebulaChatDbContext chatContext;

        public MessageRepository(NebulaChatDbContext chatContext)
        {
            this.chatContext = chatContext;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _GetMessages().ToArrayAsync();
        }
        public IEnumerable<Message> GetMessages()
        {
            return _GetMessages().ToArray();
        }
        private IQueryable<Message> _GetMessages()
        {
            return chatContext.Messages.Include(m => m.Author).Include(m => m.Recipient).OrderBy(c => c.CreatedDate);
        }

        public async Task<Message> GetMessage(Guid messageId)
        {
            return await chatContext.Messages.Include(m => m.Author).Include(m => m.Recipient).SingleAsync(c => c.Id == messageId);
        }

        public async Task InsertMessage(Message message)
        {
            message.Id = Guid.NewGuid();
            await chatContext.Messages.AddAsync(message);
            await chatContext.SaveChangesAsync();
        }
        public void Purge(int daysAfterMessagePosted)
        {
            try
            {
                var veinteHoras = DateTime.Now.AddDays(-daysAfterMessagePosted);
                var itemsToPurge = chatContext.Messages.Where(c => c.CreatedDate < veinteHoras);
                chatContext.Messages.RemoveRange(itemsToPurge);
                chatContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IEnumerable<TopTenResponse>> TopTen()
        {
            var group = await chatContext.Messages
                       .GroupBy(p => p.AuthorId)
                       .Select(g => new TempGroup { AuthorId = g.Key, Count = g.Count() })
                       .OrderByDescending(c => c.Count).Take(10).ToListAsync();


            var messages = await chatContext.Messages
            .Include(c => c.Author).ToListAsync();

            var topTen = group.Where(c => messages.Any(d => d.AuthorId == c.AuthorId))
                .Select(c => new TopTenResponse {
                    User = chatContext.Users.Where(d => d.Id == c.AuthorId).Select(d => new ChatUserDto
                    {
                        Id = d.Id,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Username = d.Username
                    }).First(),
                    MessageCount = c.Count
                });

            return topTen;
        }
    }

    struct TempGroup
    {
        public int AuthorId { get; set; }
        public int Count { get; set; }
    }
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        IEnumerable<Message> GetMessages();
        Task<Message> GetMessage(Guid MessageId);
        Task InsertMessage(Message message);
        void Purge(int daysAfterMessagePosted);
        Task<IEnumerable<TopTenResponse>> TopTen();
    }

}
