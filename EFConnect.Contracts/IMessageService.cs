using System.Collections.Generic;
using System.Threading.Tasks;
using EFConnect.Data.Entities;
using EFConnect.Models;
using EFConnect.Models.Message;

namespace EFConnect.Contracts
{
    public interface IMessageService
    {
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageToReturn>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageToReturn>> GetMessageThread(int userId, int recipientId);
        Task<MessageToReturn> CreateMessage(MessageForCreation messageForCreation);
        Task<bool> MarkMessageAsRead(Message message);
        Task<bool> SaveAll();
    }
}