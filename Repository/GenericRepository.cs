using Microsoft.EntityFrameworkCore;
using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.IRepository;
using System.Linq.Expressions;

namespace OnlineLibraryAspNet.Repository
{
    public class GenericRepository<TEntity>:IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext context;
        protected DbSet<TEntity> entities;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
            entities = context.Set<TEntity>();

        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
         => await entities.FirstOrDefaultAsync(predicate);
        public async Task<TEntity> CreateAsync(TEntity mobiUz)
        {
            var entry = await entities.AddAsync(mobiUz);
            await context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) return entities;
            return entities.Where(predicate);
        }
        public async Task<TEntity> UpdateAsync(TEntity mobiUz)
        {
            var entry = entities.Update(mobiUz);
            await context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<bool> DeleteAsync(TEntity mobiUz)
        {
            try
            {
                context.Remove(mobiUz);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
