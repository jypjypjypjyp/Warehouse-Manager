namespace MyWMS.Models
{
    public class DealEntry
    {
        public int DealId { get; set; }
        public Deal Deal { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public double Prize { get; set; }
        public double Amount { get; set; }
        public double Off { get; set; }
        public string Note { get; set; }
    }
}
