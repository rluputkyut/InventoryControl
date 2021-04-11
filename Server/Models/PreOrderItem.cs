using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class PreOrderItem
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal OrderPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual PreOrderHeader Header { get; set; }
        public virtual Product Product { get; set; }
    }
}
