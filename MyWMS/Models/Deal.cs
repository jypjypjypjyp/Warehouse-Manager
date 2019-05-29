using System;
using System.Collections.Generic;

namespace MyWMS.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int KeeperId { get; set; }
        public Keeper Keeper { get; set; }
        public int SalesmanId { get; set; }
        public Salesman Salesman { get; set; }
        public virtual ICollection<DealEntry> DealEntries { get; set; }
        
    }
}
