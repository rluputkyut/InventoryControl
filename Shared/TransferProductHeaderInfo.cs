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

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "From Warehouse is Required!")]
        public int FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "To Warehouse is Required!")]
        public int ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; }

        [Required(ErrorMessage = "Cost is Required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid Value!")]
        public decimal Cost { get; set; }
        public string Remark { get; set; }

        public DateTime TransferDate { get; set; }

        [Required(ErrorMessage = "Products are Required!"), MinLength(1,ErrorMessage = "Products are Required!")]
        public List<TransferProductItemInfo> Items { get; set; }
    }

    public class TransferProductItemInfo
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }

        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Product is Required!")]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        //[Required(ErrorMessage = "Qiantity is Required!")]
        //[Range(1, int.MaxValue, ErrorMessage = "Invalid Value!")]
        public int Quantity { get; set; }
       
    }
}
