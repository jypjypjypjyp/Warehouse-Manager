using System.Collections.Generic;

namespace MyWMS.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public virtual ICollection<WarehouseEntry> WarehouseEntries { get; set; }
    }
}
