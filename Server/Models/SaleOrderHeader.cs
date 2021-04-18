using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class SaleOrderHeader
    {
        public SaleOrderHeader()
        {
            SaleOrderItems = new HashSet<SaleOrderItem>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int WarehouseId { get; set; }
        public int CustomerId { get; set; }
        public bool CashOnDelivery { get; set; }
        public bool AccountTransfer { get; set; }
        public string TransferInformation { get; set; }
        public string Remark { get; set; }
        public bool Delivered { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<SaleOrderItem> SaleOrderItems { get; set; }
    }
}
