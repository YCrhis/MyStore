using MyStore.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyStore.Repositories
// this is a generic repository class that can be used for any entity type in the application. It provides basic CRUD operations and can be extended to include more specific methods as needed.
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _dbContext;
        public GenericRepository(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>[]? conditions = null,
            Expression<Func<TEntity, object>>[]? includes = null
            )
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (conditions is not null)
                foreach (var condition in conditions) query = query.Where(condition);

            if (includes is not null)
                foreach (var include in includes) query = query.Include(include);


            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity) 
        { 
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int identityId)
        {
            return await _dbContext.Set<TEntity>().FindAsync(identityId);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity) 
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

    }
}
