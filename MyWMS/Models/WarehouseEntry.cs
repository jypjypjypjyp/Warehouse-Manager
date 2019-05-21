namespace MyWMS.Models
{
    public class WarehouseEntry
    {
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public double Amount { get; set; }
    }
}
