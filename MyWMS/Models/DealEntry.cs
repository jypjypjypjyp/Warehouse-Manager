using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWMS.Models
{
    public class DealEntry
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Deal")]
        public int DealId { get; set; }
        public Deal Deal { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Key, Column(Order = 3)]
        public double Prize { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
    }
}
