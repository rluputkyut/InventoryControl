using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class ProductListRequest : PageParameters
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string ProductType { get; set; }
    }
    public class ProductList
    {
        public List<ProductInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }

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
        public string BatchCode { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
