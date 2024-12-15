using OnlineLibraryAspNet.DTOs;

namespace OnlineLibraryAspNet.Interfice
{
    public interface IAuthService
    {
        Task<ResponceDto<LoginResponceDto>> AuthenticateAsync(string email, string password);
        Task<ResponceDto> RegisterAsync(UserRegisterDto registerDto);
        Task<ResponceDto<string>> GetUserIdFromTokenAsync(string token);
    }
}
