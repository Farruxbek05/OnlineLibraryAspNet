using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.IRepositorys
{
    public interface ITokenRepository
    {
        Task<ResponceDto<Token>> FindRefreshToken(string refreshToken);
        Task<ResponceDto<Token>> FindAccessToken(string accessToken);
        Task<ResponceDto<Token>> FindUserById(Guid user_id);
        Task<ResponceDto> DeleteAsync(Token data);
        Task<ResponceDto<Token>> CreateAsync(Token newTokenEntry_tg);
        Task<ResponceDto<Token>> FindByIdAsync(Guid userId);
      
    }
}
