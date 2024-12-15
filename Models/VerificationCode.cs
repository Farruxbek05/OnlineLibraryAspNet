using OnlineLibraryAspNet.Enum;

namespace OnlineLibraryAspNet.Models
{
    public class VerificationCode:Entity
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public VerificationCodeType CodeType { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
