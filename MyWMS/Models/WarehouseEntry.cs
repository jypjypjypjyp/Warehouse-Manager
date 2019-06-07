using MyWMS.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWMS.Models
{
    public class WarehouseEntry : ViewModelBase
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        private double _Amount;
        public double Amount
        {
            get => _Amount;
            set => SetProperty(ref _Amount, value);
        }
        [NotMapped]
        public bool IsLoad { get; set; } = false;
    }
}
