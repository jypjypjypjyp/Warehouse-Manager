using System.Collections.Generic;

namespace MyWMS.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Unit { get; set; }
        public bool Available { get; set; }
        public virtual ICollection<WarehouseEntry> WarehouseEntries { get; set; }
        public virtual ICollection<DealEntry> DealEntries { get; set; }

        public override string ToString()
            => Specification == null ? $"{Name}" : $"{Name}({Specification})";
    }
}
