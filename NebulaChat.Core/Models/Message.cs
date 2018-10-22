using NebulaChat.Core.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NebulaChat.Core.Models
{
    public class Message : IMessage
    {
        public Message()
        {
            this.CreatedDate = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int AuthorId { get; set; }

        public int RecipientId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        #region Navigation Properties

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
        [ForeignKey("RecipientId")]
        public virtual User Recipient { get; set; }

        #endregion
    }
}
