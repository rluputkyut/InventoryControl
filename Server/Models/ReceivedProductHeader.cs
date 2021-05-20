using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class ReceivedProductHeader
    {
        public ReceivedProductHeader()
        {
            ReceivedProductItems = new HashSet<ReceivedProductItem>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }
        public decimal Cost { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<ReceivedProductItem> ReceivedProductItems { get; set; }
    }
}
