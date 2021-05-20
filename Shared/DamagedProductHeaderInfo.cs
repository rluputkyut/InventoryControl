using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class DamagedProductHeaderList
    {
        public List<DamagedProductHeaderInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }
    public class DamagedProductHeaderInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Warehouse is Required!")]
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
                

        [Required(ErrorMessage = "Cost is Required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid Value!")]
        public decimal Cost { get; set; }
        public string Remark { get; set; }
        public DateTime DamagedDate { get; set; }

        [Required(ErrorMessage = "Products are Required!"), MinLength(1, ErrorMessage = "Products are Required!")]
        public List<DamagedProductItemInfo> Items { get; set; }
    }

    public class DamagedProductItemInfo
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
