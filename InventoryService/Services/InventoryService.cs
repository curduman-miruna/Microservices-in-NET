using InventoryService.Models;
using InventoryService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public class InventorysService(IInventoryRepository repository)
    {
        public Task<IEnumerable<InventoryItem>> GetAllItemsAsync() => repository.GetAllItemsAsync();
        public Task<InventoryItem> GetItemByIdAsync(Guid id) => repository.GetItemByIdAsync(id);
        public Task<InventoryItem> CreateItemAsync(InventoryItem item) => repository.CreateItemAsync(item);
        public Task DeleteItemAsync(Guid id) => repository.DeleteItemAsync(id);
    }
}