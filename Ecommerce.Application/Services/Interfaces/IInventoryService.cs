using Ecommerce.Application.DTOs.Inventory;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<bool> IsProductAvailableAsync(int productId, int quantity);
        Task<int> GetProductStockAsync(int productId);
        Task UpdateStockAsync(int productId, int quantity, InventoryChangeType changeType, string description);
        Task<IEnumerable<InventoryLogDto>> GetInventoryLogsAsync(int productId);
        Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold);
    }
}
