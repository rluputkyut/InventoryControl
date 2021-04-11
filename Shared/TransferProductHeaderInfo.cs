using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class TransferProductHeaderInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "From Warehouse is Required!")]
        public int FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; }

        [Required(ErrorMessage = "To Warehouse is Required!")]
        public int ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; }

        [Required(ErrorMessage = "Cost is Required!")]
        public decimal Cost { get; set; }
        public string Remark { get; set; }

        public DateTime TransferDate { get; set; }
        public List<TransferProductItemInfo> Items { get; set; }
    }

    public class TransferProductItemInfo
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }

        [Required(ErrorMessage = "Product is Required!")]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Qiantity is Required!")]
        public int Quantity { get; set; }
       
    }
}
