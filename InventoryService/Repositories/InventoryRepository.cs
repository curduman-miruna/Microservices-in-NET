using InventoryService.Data;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public class InventoryRepository(InventoryDbContext context) : IInventoryRepository
    {
        public async Task<IEnumerable<InventoryItem>> GetAllItemsAsync()
        {
            return await context.InventoryItems.ToListAsync();
        }

        public async Task<InventoryItem> GetItemByIdAsync(Guid id)
        {
            return await context.InventoryItems.FindAsync(id);
        }

        public async Task<InventoryItem> CreateItemAsync(InventoryItem item)
        {
            context.InventoryItems.Add(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var item = await context.InventoryItems.FindAsync(id);
            if (item != null)
            {
                context.InventoryItems.Remove(item);
                await context.SaveChangesAsync();
            }
        }
    }
}