using InventoryService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetAllItemsAsync();
        Task<InventoryItem> GetItemByIdAsync(Guid id);
        Task<InventoryItem> CreateItemAsync(InventoryItem item);
        Task DeleteItemAsync(Guid id);
    }
}