using Microsoft.EntityFrameworkCore;
using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.IRepositorys;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Repository
{
    public class UserRepository:GenericUserRepository<User>,IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public Task<ResponceDto<User>> FindByEmailAsync(string email)
        {
            try
            {
                var data = _context.Set<User>().FirstOrDefault(x => x.Email == email);

                if (data == null)
                {
                    return Task.FromResult(new ResponceDto<User> { Success = false, Message = "Data not found" });
                }
                return Task.FromResult(new ResponceDto<User> { Data = data, Success = true });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponceDto<User> { Success = false, Message = $"Error: {ex.Message}" });

            }
        }

        public Task<ResponceDto<User>> FindByUsernameAsync(string username)
        {
            try
            {
                var data = _context.Set<User>().FirstOrDefault(x => x.Username == username.ToLower());

                if (data == null)
                {
                    return Task.FromResult(new ResponceDto<User> { Success = false, Message = "Data not found" });
                }
                return Task.FromResult(new ResponceDto<User> { Data = data, Success = true });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponceDto<User> { Success = false, Message = $"Error: {ex.Message}" });

            }
        }

        public Task<ResponceDto<User>> FindUsernameOrEmailAsync(string username, string email)
        {
            try
            {
                var data = _context.Set<User>().FirstOrDefault(x => x.Username == username.ToLower() || x.Email == email.ToLower());

                if (data == null)
                {
                    return Task.FromResult(new ResponceDto<User> { Success = false, Message = "Data not found" });
                }
                return Task.FromResult(new ResponceDto<User> { Data = data, Success = true });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponceDto<User> { Success = false, Message = $"Error: {ex.Message}" });

            }
        }

        public Task<ResponceDto<User>> FindUsernameOrEmailAsync(string username_or_password)
        {
            try
            {
                var data = _context.Set<User>().FirstOrDefault(x => x.Username == username_or_password.ToLower() ||
                                                                    x.Email == username_or_password.ToLower());

                if (data == null)
                {
                    return Task.FromResult(new ResponceDto<User> { Success = false, Message = "Data not found" });
                }
                return Task.FromResult(new ResponceDto<User> { Data = data, Success = true });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponceDto<User> { Success = false, Message = $"Error: {ex.Message}", StatusCode = 500 });
            }
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var result = await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId);
            return result;
        }

        public async Task<bool> IsTokenValidAsync(Guid userId, string token)
        {
            var userToken = await _context.Set<Token>().SingleOrDefaultAsync(ut => ut.UserId == userId && ut.AccessToken == token);
            return userToken != null && userToken.AccessTokenExpiryTime > DateTime.UtcNow;
        }

        public async Task<ResponceDto<User>> UpdateAsync(Guid UserId, UserUpdateDto UpdateUser)
        {
            var transaction = _context.Database.BeginTransactionAsync();
            try
            {
                var user = await FindByIdAsync(UserId);
                if (!user.Success)
                {
                    return new ResponceDto<User> { Success = false, Message = "Data not found" };
                }

                user.Data.FirstName = UpdateUser.FirstName;
                user.Data.LastName = UpdateUser.LastName;
                user.Data.MiddleName = UpdateUser.MiddleName;
                user.Data.Username = UpdateUser.Username;
                user.Data.Email = UpdateUser.Email;
                user.Data.TelegramId = UpdateUser.TelegramId;
                user.Data.Status = UpdateUser.Status;
                user.Data.HashPassword = UpdateUser.HashPassword;
                user.Data.Role = UpdateUser.Role;
                user.Data.Gender = UpdateUser.Gender;

                await UpdateAsync(user.Data);

                await transaction.Result.CommitAsync();

                return new ResponceDto<User> { Data = user.Data, Success = true };

            }
            catch (Exception ex)
            {
                await transaction.Result.RollbackAsync();
                return new ResponceDto<User> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
        public async Task<ResponceDto<User>> UpdateUserAsync(Guid UserId, User user)
        {
            var transaction = _context.Database.BeginTransactionAsync();
            try
            {
                var userUpdate = await FindByIdAsync(UserId);
                if (!userUpdate.Success)
                {
                    return new ResponceDto<User> { Success = false, Message = "Data not found" };
                }

                var result = await UpdateAsync(user);

                if (result.Success)
                {

                    await transaction.Result.CommitAsync();
                    return new ResponceDto<User> { Data = user, Success = true };
                }
                else
                {
                    await transaction.Result.RollbackAsync();
                    return new ResponceDto<User> { Success = false, Message = "Data not found" };
                }

            }
            catch (Exception ex)
            {
                await transaction.Result.RollbackAsync();
                return new ResponceDto<User> { Success = false, Message = $"Error: {ex.Message}" };

            }
        }
    }
}

