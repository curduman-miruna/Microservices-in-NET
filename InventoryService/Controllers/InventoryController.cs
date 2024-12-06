using InventoryService.Models;
using InventoryService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryService.Services;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventorysService _inventoryService;

        public InventoryController(InventorysService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public Task<IEnumerable<InventoryItem>> GetAllItems() => _inventoryService.GetAllItemsAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetItemById(Guid id)
        {
            var item = await _inventoryService.GetItemByIdAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItem>> CreateItem(InventoryItem item)
        {
            var createdItem = await _inventoryService.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemById), new { id = createdItem.Id }, createdItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            await _inventoryService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}