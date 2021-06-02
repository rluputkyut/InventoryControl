using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class Customer
    {
        public Customer()
        {
            PreOrderHeaders = new HashSet<PreOrderHeader>();
            SaleOrderHeaders = new HashSet<SaleOrderHeader>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string AccountInformation { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<PreOrderHeader> PreOrderHeaders { get; set; }
        public virtual ICollection<SaleOrderHeader> SaleOrderHeaders { get; set; }
    }
}
