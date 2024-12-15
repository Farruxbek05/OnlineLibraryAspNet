using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.IRepositorys
{
    public interface IUserRepository
    {
        Task<bool> IsTokenValidAsync(Guid userId, string token);
        Task<ResponceDto<User>> FindUsernameOrEmailAsync(string username, string email);
        Task<ResponceDto<User>> FindUsernameOrEmailAsync(string username_or_password);
        Task<ResponceDto<User>> FindByUsernameAsync(string username);
        Task<ResponceDto<User>> FindByEmailAsync(string email);
        Task<ResponceDto<User>> UpdateAsync(Guid UserId, UserUpdateDto UpdateUser);
        Task<ResponceDto<User>> UpdateUserAsync(Guid UserId, User user);
        Task<ResponceDto<User>> CreateAsync(User value);
        Task<ResponceDto> DeleteByIdAsync(Guid id);
        Task<ResponceDto<User>> GetByIdAsync(Guid id);
        Task<ResponceDto<List<User>>> GetAllListAsync();
        Task<User> GetUserByIdAsync(Guid userId);
    }
}


