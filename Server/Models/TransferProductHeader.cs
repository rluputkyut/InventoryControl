using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class TransferProductHeader
    {
        public TransferProductHeader()
        {
            TransferProductItems = new HashSet<TransferProductItem>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }
        public decimal Cost { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Warehouse FromWarehouse { get; set; }
        public virtual Warehouse ToWarehouse { get; set; }
        public virtual ICollection<TransferProductItem> TransferProductItems { get; set; }
    }
}
