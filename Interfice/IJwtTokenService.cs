using OnlineLibraryAspNet.Models;
using System.Security.Claims;

namespace OnlineLibraryAspNet.Interfice
{
    public interface IJwtTokenService
    {
        Task<(string, string)> GenerateTokenAsync(User user, long? telegram_id = null);
        ClaimsPrincipal ValidateToken(string token);
        Task<(string, string)> RefreshTokenAsync(string token, string refreshToken, long? telegram_id = null);
    }
}
