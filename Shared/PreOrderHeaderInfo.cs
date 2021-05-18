using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class PreOrderHeaderList
    {
        public List<PreOrderHeaderInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }
    public class PreOrderHeaderInfo
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

        [Range(0, int.MaxValue, ErrorMessage = "Invalid Deposit!")]
        public decimal Deposit { get; set; }
        public string Remark { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Waiting Day(s)!")]
        public int WaitingDays { get; set; }
        public DateTime OrderDate { get; set; }
        public bool SoldOut { get; set; }

        [Required(ErrorMessage = "Products are Required!"), MinLength(1, ErrorMessage = "Products are Required!")]
        public List<PreOrderItemInfo> Items { get; set; }
    }

    public class PreOrderItemInfo
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }

        [Required(ErrorMessage = "Product is Required!")]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Qiantity is Required!")]
        public int Quantity { get; set; }
    }

}
