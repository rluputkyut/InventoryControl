﻿using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryControl.Server.Models
{
    public partial class DamagedProductHeader
    {
        public DamagedProductHeader()
        {
            DamagedProductItems = new HashSet<DamagedProductItem>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int WarehouseId { get; set; }
        public decimal Cost { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<DamagedProductItem> DamagedProductItems { get; set; }
    }
}
