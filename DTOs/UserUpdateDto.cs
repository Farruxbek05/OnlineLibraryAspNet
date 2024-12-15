using OnlineLibraryAspNet.Enum;

namespace OnlineLibraryAspNet.DTOs
{
    public class UserUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public long? TelegramId { get; set; }
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public string HashPassword { get; set; }
    }
}
