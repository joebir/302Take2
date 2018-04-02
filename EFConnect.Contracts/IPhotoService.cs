using System.Threading.Tasks;
using EFConnect.Data.Entities;
using EFConnect.Models.Photo;

namespace EFConnect.Contracts
{
    public interface IPhotoService
    {
        Task<PhotoForReturn> GetPhoto(int id);
        Task<PhotoForReturn> AddPhotoForUser(int userId, PhotoForCreation photoDto);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<bool> SetMainPhotoForUser(int userId, PhotoForReturn photo);
        object DeletePhotoFromCloudinary(string publicId);
        Task<Photo> GetPhotoEntity(int id);
        Task<bool> SaveAll();
    }
}