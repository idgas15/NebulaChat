namespace NebulaChat.Core.Dto
{
    public class CreateMessageRequest
    {
        public string AuthorId { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
    }
}
