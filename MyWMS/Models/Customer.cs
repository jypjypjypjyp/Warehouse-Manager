using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWMS.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Money { get; set; }
        public bool Available { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
