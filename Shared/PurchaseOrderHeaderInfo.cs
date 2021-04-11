using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class PurchaseOrderHeaderInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Warehouse is Required!")]
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "Source is Required!")]
        public string Source { get; set; }
        public string Remark { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<PurchaseOrderItemInfo> Items { get; set; }
    }

    public class PurchaseOrderItemInfo
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }

        [Required(ErrorMessage = "Product is Required!")]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Buying Price is Required!")]
        public decimal BuyingPrice { get; set; }
        public string Currency { get; set; }

        [Required(ErrorMessage = "Qiantity is Required!")]
        public int Quantity { get; set; }
    }

}
