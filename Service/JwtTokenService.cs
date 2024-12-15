using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineLibraryAspNet.Class;
using OnlineLibraryAspNet.Extensions;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.IRepositorys;
using OnlineLibraryAspNet.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace OnlineLibraryAspNet.Service
{
    public class JwtTokenService:IJwtTokenService
    {
        private readonly JwtOption _jwtOptions;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _context;
        private readonly ITokenRepository _tokenRepository;

        public JwtTokenService(IOptions<JwtOption> jwtOptions, IConfiguration configuration,
                               IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
                               ITokenRepository tokenRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _config = configuration;
            _context = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _tokenRepository = tokenRepository;
        }


        public async Task<(string?, string?)> GenerateTokenAsync(User user, long? TelegramId = null)
        {
            var User = await _tokenRepository.FindUserById(user.Id);

            if (User.Success)
            {
                var result = await _tokenRepository.DeleteAsync(User.Data);

                if (!result.Success)
                {
                    return new(null, null);
                }

            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("Role", user.Role.ToString()),
            new Claim("IpAddress", _httpContextAccessor.HttpContext.GetClientIpAddress())
        };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtOptions:Key").Value!));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _config.GetSection("JwtOptions:Issuer").Value!,
                audience: _config.GetSection("JwtOptions:Audience").Value!,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config.GetSection("JwtOptions:ExpiresInMinutes").Value!)),
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            var newAccessToken = tokenHandler.WriteToken(tokenOptions);

            var newRefreshToken = GenerateRefreshToken(user.Id);

            var newTokenEntry_tg = new Token
                   (
                       userId: user.Id,
                       accessToken: newAccessToken,
                       accessTokenExpiryTime: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config.GetSection("JwtOptions:ExpiresInMinutes").Value!)),
                       refreshToken: newRefreshToken,
                       refreshTokenExpiryTime: DateTime.UtcNow.AddMonths(1),
                       telegramID: TelegramId,
                       isLoginTelegram: true,
                       user: user
                   );

            await _tokenRepository.CreateAsync(newTokenEntry_tg);

            return (newAccessToken, newRefreshToken);
        }
        private string GenerateRefreshToken(Guid userId)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtOptions:Key").Value!));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // Fix for CS1503
            new Claim("IssuedAt", DateTime.UtcNow.ToString()), // Fix for CS1503
            new Claim("ExpiresAt", DateTime.UtcNow.AddMonths(1).ToString()) // Fix for CS1503
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: _config.GetSection("JwtOptions:Issuer").Value!,
                audience: _config.GetSection("JwtOptions:Audience").Value!,
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.WriteToken(tokenOptions);

            return jwtToken;
        }
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
        public async Task<(string, string)> RefreshTokenAsync(string token, string refreshToken, long? telegram_id = null)
        {
            try
            {
                // Refresh tokenni tekshirish
                var storedToken = await _tokenRepository.FindRefreshToken(refreshToken);

                if (storedToken == null || storedToken.Data.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    throw new SecurityTokenException("Invalid or expired refresh token.");
                }

                // Foydalanuvchini olish
                var user = await _tokenRepository.FindByIdAsync(storedToken.Data.UserId);

                if (user == null)
                {
                    throw new SecurityTokenException("Invalid user.");
                }

                User users = new();

                // Yangi JWT token va refresh tokenni yaratish
                (string newAccessToken, string newRefreshToken) = await GenerateTokenAsync(users, telegram_id); // Userni o'zgartirish kerak

                // Eski tokenni o'chirish
                await _tokenRepository.DeleteAsync(storedToken.Data);

                if (telegram_id != null)
                {
                    // Yangi token ob'yektini yaratish
                    var newTokenEntry_tg = new Token
                    (
                        userId: users.Id,
                        accessToken: newAccessToken,
                        accessTokenExpiryTime: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config.GetSection("JwtOptions:ExpiresInMinutes").Value!)),
                        refreshToken = newRefreshToken,
                        refreshTokenExpiryTime: DateTime.UtcNow.AddMonths(1),
                        telegramID: telegram_id,
                        isLoginTelegram: true,
                        user: users
                    );

                    await _tokenRepository.CreateAsync(newTokenEntry_tg);

                }
                else
                {
                    // Yangi token ob'yektini yaratish
                    var newTokenEntry = new Token
                    (
                        userId: users.Id,
                        accessToken: newAccessToken,
                        accessTokenExpiryTime: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config.GetSection("JwtOptions:ExpiresInMinutes").Value!)),
                        refreshToken = newRefreshToken,
                        refreshTokenExpiryTime: DateTime.UtcNow.AddMonths(1),
                        telegramID: null,
                        isLoginTelegram: false,
                        user: users
                    );

                    await _tokenRepository.CreateAsync(newTokenEntry);
                }

                await _context.SaveChangesAsync();


                return (newAccessToken, newRefreshToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency conflict occurred.", ex);
            }
        }
    }
}
