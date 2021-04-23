using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class WarehouseProductList
    {
        public List<WarehouseProductInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }
    public class WarehouseProductInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Warehouse is Required!")]
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Product is Required!")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity is Required!")]
        public int Quantity { get; set; }

    }
}
