using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public class InventoryLogDto
    {
        public int LogID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public InventoryChangeType ChangeType { get; set; }
        public int QuantityChanged { get; set; }
        public int NewStockLevel { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
