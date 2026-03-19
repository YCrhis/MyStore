using MyStore.Context;
using Microsoft.EntityFrameworkCore;

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

    }
}
