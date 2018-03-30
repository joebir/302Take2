using System.Threading.Tasks;
using EFConnect.Models.Photo;

namespace EFConnect.Contracts
{
    public interface IPhotoService
    {
        Task<PhotoForReturn> GetPhoto(int id);
        Task<PhotoForReturn> AddPhotoForUser(int userId, PhotoForCreation photoDto);
    }
}