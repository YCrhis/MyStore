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


        public async Task<List<OrderVM>> GetAllWithDetailsAsync(int userId)
        {
            var oders = await _Ordenrepository.GetAllWithDetailsAsync(userId);
            var ordersVM = oders.Select(o => new OrderVM()
            {
                OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                TotalAmount = o.TotalAmount.ToString("C2"),
                OrdenItems = o.OrdenItems.Select(oi => new OrdenItemVM()
                {
                    Name = oi?.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price.ToString("C")
                }).ToList()
            }).ToList();

            return ordersVM;
        }
    }
}
