using AngleSharp.Dom;

namespace OnlineLibraryAspNet.Models
{
    public class Token:Entity
    {
        public Token() { }

        public Guid UserId { get; private set; }
        public string AccessToken { get; private set; }
        public DateTime AccessTokenExpiryTime { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime RefreshTokenExpiryTime { get; private set; }
        public long? TelegramId { get; private set; }
        public bool IsLoginTelegram { get; private set; } = false;
        public DateTime CreateAt { get; private set; } = DateTime.UtcNow;
        public virtual User User { get; set; }

        public Token(Guid userId, string accessToken, DateTime accessTokenExpiryTime, string refreshToken, DateTime refreshTokenExpiryTime, long? telegramID, User user, bool isLoginTelegram = false)
        {
            UserId = userId;
            AccessToken = accessToken;
            AccessTokenExpiryTime = accessTokenExpiryTime;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
            User = user;
            IsLoginTelegram = isLoginTelegram;
            TelegramId = telegramID;
        }
    }
}
