using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class PurchaseOrderItem
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal BuyingPrice { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual PurchaseOrderHeader Header { get; set; }
        public virtual Product Product { get; set; }
    }
}
