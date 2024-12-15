using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Enum;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.Models;
using OnlineLibraryAspNet.Repository;

public class VerificationCodeRepository :GenericUserRepository<VerificationCode> ,IVerificationCodeRepository
    
{
    private readonly AppDbContext _context;

    public VerificationCodeRepository(AppDbContext context):base(context)
    {
        _context = context;
    }


    public Task<ResponceDto<VerificationCode>> GetLastVerificationCodeAsync(string toEmail, VerificationCodeType codeType)
    {
        throw new NotImplementedException();
    }

    public Task<ResponceDto<VerificationCode>> GetVerificationCodeAsync(string toEmail, string code, VerificationCodeType codeType)
    {
        throw new NotImplementedException();
    }
}