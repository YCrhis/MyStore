using MyStore.Entities;
using MyStore.Models;
using MyStore.Repositories;

namespace MyStore.Services
{
    public class OrderService
    {

        private readonly OrderRepository _Ordenrepository;
        public OrderService(OrderRepository orderRepository) 
        {
            _Ordenrepository = orderRepository;
        }

        public async Task AddAsync(List<CartItemVM> cartItemVM, int userId)
        {
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = userId,
                TotalAmount = cartItemVM.Sum(x => x.Price * x.Quantity),
                OrdenItems = cartItemVM.Select(x => new OrderItem()
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price,
                }).ToList()
            };

            await _Ordenrepository.AddAsync(order);
        }
    }
}
