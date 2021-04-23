using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class CustomerList
    {
        public List<CustomerInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }

    public class CustomerInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "NickName is Required!")]
        public string NickName { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone No!")]
        [Required(ErrorMessage = "PhoneNo is Required!")]
        public string PhoneNo { get; set; }

        public string Address { get; set; }
        public string AccountInformation { get; set; }
    }
}
