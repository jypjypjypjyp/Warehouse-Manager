using MyWMS.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWMS.Models
{
    public class Item : ViewModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        private string _Unit;
        public string Unit
        {
            get => _Unit;
            set => SetProperty(ref _Unit, value);
        }

        public double Load { get; set; }
        public bool Available { get; set; }
        public virtual ICollection<WarehouseEntry> WarehouseEntries { get; set; }
        public virtual ICollection<DealEntry> DealEntries { get; set; }

        public override string ToString()
            => Specification == null ? $"{Name}" : $"{Name}({Specification})";
    }
}
