using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class Warehouse
    {
        public Warehouse()
        {
            PurchaseOrderHeaders = new HashSet<PurchaseOrderHeader>();
            SaleOrderHeaders = new HashSet<SaleOrderHeader>();
            TransferProductHeaderFromWarehouses = new HashSet<TransferProductHeader>();
            TransferProductHeaderToWarehouses = new HashSet<TransferProductHeader>();
            WarehouseProducts = new HashSet<WarehouseProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public virtual ICollection<SaleOrderHeader> SaleOrderHeaders { get; set; }
        public virtual ICollection<TransferProductHeader> TransferProductHeaderFromWarehouses { get; set; }
        public virtual ICollection<TransferProductHeader> TransferProductHeaderToWarehouses { get; set; }
        public virtual ICollection<WarehouseProduct> WarehouseProducts { get; set; }
    }
}
