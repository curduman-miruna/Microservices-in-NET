using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await context.Orders.FindAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await context.Orders.FindAsync(id);
            if (order != null)
            {
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
            }
        }
    }
}