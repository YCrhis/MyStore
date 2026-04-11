using Microsoft.EntityFrameworkCore;
using MyStore.Context;
using MyStore.Entities;

namespace MyStore.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        private readonly AppDbContext _dbContext;
        public OrderRepository(AppDbContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public override async Task AddAsync(Order entity)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach(var detail in entity.OrdenItems)
                {
                    var product = await _dbContext.Product.FindAsync(detail.ProductId);
                    product?.Stock -= detail.Quantity;

                }

                await _dbContext.Order.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex) 
            { 
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync(int userId)
        {
            var orders = await _dbContext.Order
                .Where(o => o.UserId == userId)
                .Include(o => o.OrdenItems)
                .ThenInclude(x => x.Product)
                .ToListAsync();
            return orders;
        }

    }
}
