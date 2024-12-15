using System.Linq.Expressions;

namespace OnlineLibraryAspNet.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);

        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> expression);

        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
