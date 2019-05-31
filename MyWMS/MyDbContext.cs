using Microsoft.EntityFrameworkCore;
using MyWMS.Models;

namespace MyWMS
{
    public class MyDbContext : DbContext
    {
        public static MyDbContext Instance => new MyDbContext();

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseEntry> WarehouseEntries { get; set; }
        public DbSet<Keeper> Keepers { get; set; }
        public DbSet<Salesman> Salesmen { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealEntry> DealEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=wms.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Warehouse>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Keeper>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Salesman>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Item>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Deal>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Warehouse>().HasKey(a => a.Id);

            builder.Entity<WarehouseEntry>().HasKey(a => new { a.WarehouseId, a.ItemId });
            builder.Entity<WarehouseEntry>().HasOne(a => a.Warehouse).WithMany(a => a.WarehouseEntries).HasForeignKey(a => a.WarehouseId);
            builder.Entity<WarehouseEntry>().HasOne(a => a.Item).WithMany(a => a.WarehouseEntries).HasForeignKey(a => a.ItemId);

            builder.Entity<Keeper>().HasKey(a => a.Id);

            builder.Entity<Salesman>().HasKey(a => a.Id);

            builder.Entity<Item>().HasKey(a => a.Id);

            builder.Entity<Deal>().HasKey(a => a.Id);
            builder.Entity<Deal>().HasOne(a => a.Keeper).WithMany(a => a.Deals).HasForeignKey(a => a.KeeperId);
            builder.Entity<Deal>().HasOne(a => a.Salesman).WithMany(a => a.Deals).HasForeignKey(a => a.SalesmanId);
            builder.Entity<Deal>().HasOne(a => a.Warehouse).WithMany(a => a.Deals).HasForeignKey(a => a.WarehouseId);

            builder.Entity<DealEntry>().HasKey(a => new { a.DealId, a.ItemId });
            builder.Entity<DealEntry>().HasOne(a => a.Item).WithMany(a => a.DealEntries).HasForeignKey(a => a.ItemId);
            builder.Entity<DealEntry>().HasOne(a => a.Deal).WithMany(a => a.DealEntries).HasForeignKey(a => a.DealId);
        }
    }
}
