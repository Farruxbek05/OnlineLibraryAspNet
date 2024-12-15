using OnlineLibraryAspNet.DTOs;

namespace OnlineLibraryAspNet.Interfice
{
    public interface IEmailService
    {
        Task<ResponceDto> SendEmailAsync(string toEmail, string subject, string message);
        Task<ResponceDto> SendWelcomeEmailAsync(string toEmail, string fullname, string username, string password);
        Task<ResponceDto> SendVerificationEmailAsync(string toEmail, string verificationCode);
        Task<ResponceDto> VerifyEmailAsync(string email, string verificationCode);
        Task<ResponceDto> ResetPasswordRequest(string ToEmail, string verificationCode);
        Task<ResponceDto> ResetMyPassword(string Email, string VerificationCode, string Password);
        string GenerateVerificationCode(int length = 6);
    }
}
