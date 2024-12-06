using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(OrdersService orderService) : ControllerBase
    {
        [HttpGet]
        public Task<IEnumerable<Order>> GetAllOrders() => orderService.GetAllOrdersAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            var createdOrder = await orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}