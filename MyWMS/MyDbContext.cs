using MyWMS.Models;
using System.Data.Entity;

namespace MyWMS
{
    public class MyDbContext : DbContext
    {
        public static MyDbContext Instance => new MyDbContext();

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseEntry> WarehouseEntries { get; set; }
        public DbSet<Keeper> Keepers { get; set; }
        public DbSet<Salesman> Salesmen { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealEntry> DealEntries { get; set; }

        public MyDbContext() :
            base("Default")
        {
            //disable initializer
            Database.SetInitializer<MyDbContext>(null);
        }
    }
}
