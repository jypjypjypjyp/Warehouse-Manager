using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWMS.Models
{
    public class Deal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public bool InOrOut { get; set; }
        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        [ForeignKey("Keeper")]
        public int KeeperId { get; set; }
        public Keeper Keeper { get; set; }
        [ForeignKey("Salesman")]
        public int SalesmanId { get; set; }
        public Salesman Salesman { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public virtual ICollection<DealEntry> DealEntries { get; set; }

        public string GetNumber()
        {
            return $"XK-000-{Time.Year}-{Time.Month}-{Time.Day}-{Id}";
        }
    }
}
