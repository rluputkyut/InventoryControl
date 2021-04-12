using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class ProductInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is Required!")]
        public string Description { get; set; }

        public string Photo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Brand is Required!")]
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ProductType is Required!")]
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }

        public decimal? Size { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price is Required!")]
        public decimal? Price { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
