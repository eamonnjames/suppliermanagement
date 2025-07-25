using Microsoft.EntityFrameworkCore;
using SupplierManagement.Core.Entities;

namespace SupplierManagement.Infrastructure.Data
{
    public class SupplierDbContext : DbContext
    {
        public SupplierDbContext(DbContextOptions<SupplierDbContext> options) : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierRate> SupplierRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId);
                entity.Property(e => e.SupplierId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(450);
                entity.Property(e => e.Address).HasMaxLength(450);
                entity.Property(e => e.CreatedByUser).IsRequired().HasMaxLength(450);
                entity.Property(e => e.CreatedOn).IsRequired();
            });

            // Configure SupplierRate entity
            modelBuilder.Entity<SupplierRate>(entity =>
            {
                entity.HasKey(e => e.SupplierRateId);
                entity.Property(e => e.SupplierRateId).ValueGeneratedOnAdd();
                entity.Property(e => e.Rate).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.RateStartDate).IsRequired();
                entity.Property(e => e.CreatedByUser).IsRequired().HasMaxLength(450);
                entity.Property(e => e.CreatedOn).IsRequired();

                // Configure foreign key relationship
                entity.HasOne(sr => sr.Supplier)
                    .WithMany(s => s.SupplierRates)
                    .HasForeignKey(sr => sr.SupplierId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, Name = "BestValue", Address = "1, Main Street, The District, City1, XXX-AADA", CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new Supplier { SupplierId = 2, Name = "Quality Corp", Address = "2, High Street, Downtown, City2, YYY-BBBB", CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new Supplier { SupplierId = 3, Name = "Premium Ltd", Address = "3, Park Avenue, Uptown, City3, ZZZ-CCCC", CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new Supplier { SupplierId = 4, Name = "Overlap Testing Corp", Address = "4, Test Street, Test City", CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) }
            );

            // Seed SupplierRates based on test data
            modelBuilder.Entity<SupplierRate>().HasData(
                // Supplier 1 rates - no overlaps (contiguous/gaps)
                new SupplierRate { SupplierRateId = 1, SupplierId = 1, Rate = 10, RateStartDate = new DateTime(2015, 1, 1), RateEndDate = new DateTime(2015, 3, 31), CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new SupplierRate { SupplierRateId = 2, SupplierId = 1, Rate = 20, RateStartDate = new DateTime(2015, 4, 1), RateEndDate = new DateTime(2015, 5, 1), CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new SupplierRate { SupplierRateId = 3, SupplierId = 1, Rate = 10, RateStartDate = new DateTime(2015, 5, 30), RateEndDate = new DateTime(2015, 7, 25), CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new SupplierRate { SupplierRateId = 4, SupplierId = 1, Rate = 25, RateStartDate = new DateTime(2015, 10, 1), RateEndDate = null, CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },

                // Supplier 2 rates - no overlaps (single rate)
                new SupplierRate { SupplierRateId = 5, SupplierId = 2, Rate = 100, RateStartDate = new DateTime(2016, 11, 1), RateEndDate = null, CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },

                // Supplier 3 rates - has actual overlaps to test the functionality
                new SupplierRate { SupplierRateId = 6, SupplierId = 3, Rate = 30, RateStartDate = new DateTime(2016, 12, 1), RateEndDate = new DateTime(2017, 1, 1), CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new SupplierRate { SupplierRateId = 7, SupplierId = 3, Rate = 30, RateStartDate = new DateTime(2017, 1, 2), RateEndDate = null, CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },

                // Additional test rates with actual overlaps for Supplier 3
                new SupplierRate { SupplierRateId = 8, SupplierId = 3, Rate = 35, RateStartDate = new DateTime(2016, 12, 15), RateEndDate = new DateTime(2017, 1, 15), CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },

                // Supplier 4 with overlapping rates for testing
                new SupplierRate { SupplierRateId = 9, SupplierId = 4, Rate = 50, RateStartDate = new DateTime(2020, 1, 1), RateEndDate = new DateTime(2020, 6, 30), CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new SupplierRate { SupplierRateId = 10, SupplierId = 4, Rate = 60, RateStartDate = new DateTime(2020, 3, 1), RateEndDate = new DateTime(2020, 9, 30), CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) },
                new SupplierRate { SupplierRateId = 11, SupplierId = 4, Rate = 55, RateStartDate = new DateTime(2020, 8, 1), RateEndDate = null, CreatedByUser = "System.Admin", CreatedOn = new DateTime(2021, 7, 30) }
            );
        }
    }
}
