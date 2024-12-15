using OnlineLibraryAspNet.Enum;
using System.Reflection;

namespace OnlineLibraryAspNet.Models
{
    public class User:Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public Guid PhotoId { get; set; }
        public float Balance { get; set; } = 0;
        public bool IsEmailConfirmed { get; set; } = false;
        public long? TelegramId { get; set; }
        public UserRole Role { get; set; } = UserRole.Unknown;
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public string HashPassword { get; set; }
    }
}
