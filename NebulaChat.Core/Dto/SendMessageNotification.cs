using NebulaChat.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NebulaChat.Core.Dto
{
    public class SendMessageNotification
    {
        public ChatUserDto Author { get; set; }
        public ChatUserDto Recipient { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
