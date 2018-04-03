using System.Collections.Generic;
using System.Threading.Tasks;
using EFConnect.Data.Entities;
using EFConnect.Models;
using EFConnect.Models.User;

namespace EFConnect.Contracts
{
    public interface IUserService
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<UserForList>> GetUsers(UserParams userParams);
        Task<UserForDetail> GetUser(int id);
        Task<bool> UpdateUser(int id, UserForUpdate userForUpdate);
        Task<User> GetUserEntity(int id);
    }
}