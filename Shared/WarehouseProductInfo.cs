using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class WarehouseProductInfo
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "Product is Required!")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Quantity is Required!")]
        public int Quantity { get; set; }

    }
}
