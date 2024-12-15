using Microsoft.EntityFrameworkCore;
using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.IRepositorys;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Repository
{
    public class TokenRepository :GenericUserRepository<Token>, ITokenRepository
    {
        private readonly AppDbContext _context;

        public TokenRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<ResponceDto<Token>> FindAccessToken(string accessToken)
        {
            var token = await _context.Set<Token>().FirstOrDefaultAsync(x => x.AccessToken == accessToken);
            if (token == null)
            {
                return new ResponceDto<Token> { Success = false, Message = "Token not found" };
            }
            return new ResponceDto<Token> { Success = true, Data = token };
        }

        public async Task<ResponceDto<Token>> FindRefreshToken(string refreshToken)
        {
            var token = await _context.Set<Token>().FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (token == null)
            {
                return new ResponceDto<Token> { Success = false, Message = "Token not found" };
            }
            return new ResponceDto<Token> { Success = true, Data = token };
        }

        public async Task<ResponceDto<Token>> FindUserById(Guid user_id)
        {
            var token = await _context.Set<Token>().FirstOrDefaultAsync(x => x.UserId == user_id);
            if (token == null)
            {
                return new ResponceDto<Token> { Success = false, Message = "User not found" };
            }
            return new ResponceDto<Token> { Success = true, Data = token };
        }
    }
}
