using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared
{
    public class CustomerInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "NickName is Required!")]
        public string NickName { get; set; }

        public string PhoneNo { get; set; }

        public string Address { get; set; }
        public string AccountInformation { get; set; }
    }
}
