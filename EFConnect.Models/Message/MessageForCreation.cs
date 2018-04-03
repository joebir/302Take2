using System;

namespace EFConnect.Models.Message
{
    public class MessageForCreation
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime DateSent { get; set; }
        public string Content { get; set; }

        public MessageForCreation()
        {
            DateSent = DateTime.Now;
        }
    }
}