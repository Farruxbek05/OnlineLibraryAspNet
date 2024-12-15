using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Enum;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Interfice
{
    public interface IVerificationCodeRepository
    {
        Task<ResponceDto<VerificationCode>> CreateAsync(VerificationCode verificationRecord);
        Task<ResponceDto> DeleteByIdAsync(Guid id);
        Task<ResponceDto<VerificationCode>> GetLastVerificationCodeAsync(string toEmail, VerificationCodeType codeType);
        Task<ResponceDto<VerificationCode>> GetVerificationCodeAsync(string toEmail, string code, VerificationCodeType codeType);
    }
}
