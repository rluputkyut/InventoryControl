using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class BrandList
    {
        public List<BrandInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }

    public class BrandInfo
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

    }
}
