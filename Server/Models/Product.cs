using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class Product
    {
        public Product()
        {
            DamagedProductItems = new HashSet<DamagedProductItem>();
            PreOrderItems = new HashSet<PreOrderItem>();
            PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
            SaleOrderItems = new HashSet<SaleOrderItem>();
            TransferProductItems = new HashSet<TransferProductItem>();
            WarehouseProducts = new HashSet<WarehouseProduct>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
        public decimal? Size { get; set; }
        public string BatchCode { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<DamagedProductItem> DamagedProductItems { get; set; }
        public virtual ICollection<PreOrderItem> PreOrderItems { get; set; }
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public virtual ICollection<SaleOrderItem> SaleOrderItems { get; set; }
        public virtual ICollection<TransferProductItem> TransferProductItems { get; set; }
        public virtual ICollection<WarehouseProduct> WarehouseProducts { get; set; }
    }
}
