using Microsoft.EntityFrameworkCore;
using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.DTOs;
using System.Linq.Expressions;

namespace OnlineLibraryAspNet.Repository
{
    public class GenericUserRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponceDto<T>> CreateAsync(T value)
        {
            try
            {

                 await _context.Set<T>().AddAsync(value);
                await _context.SaveChangesAsync();
                return new ResponceDto<T> { Data = value, Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto<T> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponceDto<List<T>>> GetAllListAsync()
        {
            try
            {
                var data = await _context.Set<T>().ToListAsync();
                return new ResponceDto<List<T>> { Data = data, Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto<List<T>> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponceDto<T>> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _context.Set<T>().FindAsync(id);
                if (data == null)
                {
                    return new ResponceDto<T> { Success = false, Message = "Data not found" };
                }
                return new ResponceDto<T> { Data = data, Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto<T> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponceDto<T>> UpdateAsync(T value)
        {
            try
            {
                _context.Set<T>().Update(value);
                await _context.SaveChangesAsync();
                return new ResponceDto<T> { Data = value, Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto<T> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponceDto> DeleteByIdAsync(Guid id)
        {
            try
            {
                var data = await _context.Set<T>().FindAsync(id);
                if (data == null)
                {
                    return new ResponceDto { Success = false, Message = "Data not found" };
                }

                _context.Set<T>().Remove(data);
                await _context.SaveChangesAsync();
                return new ResponceDto { Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponceDto<T>> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var data = await _context.Set<T>().FirstOrDefaultAsync(predicate);
                if (data == null)
                {
                    return new ResponceDto<T> { Success = false, Message = "Data not found" };
                }
                return new ResponceDto<T> { Data = data, Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto<T> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponceDto> DeleteAsync(T value)
        {
            try
            {
                _context.Set<T>().Remove(value);
                await _context.SaveChangesAsync();
                return new ResponceDto { Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponceDto<T>> FindByIdAsync(Guid id)
        {
            try
            {
                var data = await _context.Set<T>().FindAsync(id);

                if (data == null)
                {
                    return new ResponceDto<T> { Success = false, Message = "Data not found" };
                }

                return new ResponceDto<T> { Data = data, Success = true };
            }
            catch (Exception ex)
            {
                return new ResponceDto<T> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
    }

}
