using System.Collections.Generic;

namespace MyWMS.Models
{
    public class Keeper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
    }
}
