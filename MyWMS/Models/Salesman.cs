using System.Collections.Generic;

namespace MyWMS.Models
{
    public class Salesman
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
    }
}
