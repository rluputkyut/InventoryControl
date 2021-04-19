using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class SaleOrderHeaderList
    {
        public List<SaleOrderHeaderInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }
    public class SaleOrderHeaderInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Warehouse is Required!")]
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Customer is Required!")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsCOD { get; set; }
        public bool IsAccountTransfer { get; set; }
                
        public string TransferInfo { get; set; }
        public string Remark { get; set; }
        public DateTime SellingDate { get; set; }
        public bool Delivered { get; set; }

        [Required(ErrorMessage = "Products are Required!"), MinLength(1, ErrorMessage = "Products are Required!")]
        public List<SaleOrderItemInfo> Items { get; set; }
    }

    public class SaleOrderItemInfo
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }

        [Required(ErrorMessage = "Product is Required!")]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Selling Price is Required!")]
        public decimal SellingPrice { get; set; }
        public decimal OtherExpense { get; set; }

        [Required(ErrorMessage = "Qiantity is Required!")]
        public int Quantity { get; set; }
    }

}
