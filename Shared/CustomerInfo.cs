using InventoryControl.Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class CustomerListRequest : PageParameters
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Account { get; set; }
    }
    public class CustomerList
    {
        public List<CustomerInfo> Items { get; set; }
        public MetaData Meta { get; set; }
    }

    public class CustomerInfo
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "NickName is Required!")]
        public string NickName { get; set; }

        public CustomerType Type { get; set; }
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone No!")]
        [Required(ErrorMessage = "PhoneNo is Required!")]
        public string PhoneNo { get; set; }

        public string Address { get; set; }
        public string AccountInformation { get; set; }
    }

    public enum CustomerType
    {
        [Display(Name = "Normal")]
        Normal = 0,
        [Display(Name = "Bad")]
        Bad = 1,
        [Display(Name = "Good")]
        Good = 2
    }
}
