using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class PreOrderHeader
    {
        public PreOrderHeader()
        {
            PreOrderItems = new HashSet<PreOrderItem>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int WaitingDay { get; set; }
        public decimal Deposit { get; set; }
        public int CustomerId { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<PreOrderItem> PreOrderItems { get; set; }
    }
}
