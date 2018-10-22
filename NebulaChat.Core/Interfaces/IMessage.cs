using System;

namespace NebulaChat.Core.Models.Interfaces
{
    public interface IMessage
    {
        Guid Id { get; set; }
        string Content { get; set; }
        int AuthorId { get; set; }
        int RecipientId { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
