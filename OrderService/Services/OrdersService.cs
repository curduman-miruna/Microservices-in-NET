using OrderService.Models;
using OrderService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public class OrdersService(IOrderRepository repository)
    {
        public Task<IEnumerable<Order>> GetAllOrdersAsync() => repository.GetAllOrdersAsync();
        public Task<Order> GetOrderByIdAsync(Guid id) => repository.GetOrderByIdAsync(id);
        public Task<Order> CreateOrderAsync(Order order) => repository.CreateOrderAsync(order);
        public Task DeleteOrderAsync(Guid id) => repository.DeleteOrderAsync(id);
    }
}