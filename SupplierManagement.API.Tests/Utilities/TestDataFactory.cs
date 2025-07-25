using SupplierManagement.Core.DTOs;
using SupplierManagement.Core.Entities;

namespace SupplierManagement.API.Tests.Utilities
{
    public static class TestDataFactory
    {
        public static Supplier CreateSupplier(int id = 1, string name = "Test Supplier")
        {
            return new Supplier
            {
                SupplierId = id,
                Name = name,
                Address = $"Address {id}",
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now,
                SupplierRates = new List<SupplierRate>()
            };
        }

        public static SupplierRate CreateSupplierRate(int id = 1, int supplierId = 1, decimal rate = 100.00m)
        {
            return new SupplierRate
            {
                SupplierRateId = id,
                SupplierId = supplierId,
                Rate = rate,
                RateStartDate = DateTime.Today,
                RateEndDate = DateTime.Today.AddDays(30),
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now
            };
        }

        public static SupplierDto CreateSupplierDto(int id = 1, string name = "Test Supplier")
        {
            return new SupplierDto
            {
                SupplierId = id,
                Name = name,
                Address = $"Address {id}",
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now
            };
        }

        public static SupplierRateDto CreateSupplierRateDto(int id = 1, int supplierId = 1, decimal rate = 100.00m)
        {
            return new SupplierRateDto
            {
                SupplierRateId = id,
                SupplierId = supplierId,
                Rate = rate,
                RateStartDate = DateTime.Today,
                RateEndDate = DateTime.Today.AddDays(30),
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now
            };
        }

        public static SupplierWithRatesDto CreateSupplierWithRatesDto(int id = 1, string name = "Test Supplier")
        {
            return new SupplierWithRatesDto
            {
                SupplierId = id,
                Name = name,
                Address = $"Address {id}",
                CreatedByUser = "TestUser",
                CreatedOn = DateTime.Now,
                Rates = new List<SupplierRateDto>
                {
                    CreateSupplierRateDto(1, id)
                }
            };
        }

        public static CreateSupplierDto CreateCreateSupplierDto(string name = "New Supplier")
        {
            return new CreateSupplierDto
            {
                Name = name,
                Address = "New Address",
                CreatedByUser = "TestUser"
            };
        }

        public static UpdateSupplierDto CreateUpdateSupplierDto(string name = "Updated Supplier")
        {
            return new UpdateSupplierDto
            {
                Name = name,
                Address = "Updated Address"
            };
        }

        public static CreateSupplierRateDto CreateCreateSupplierRateDto(int supplierId = 1, decimal rate = 150.00m)
        {
            return new CreateSupplierRateDto
            {
                SupplierId = supplierId,
                Rate = rate,
                RateStartDate = DateTime.Today,
                RateEndDate = DateTime.Today.AddDays(60),
                CreatedByUser = "TestUser"
            };
        }

        public static UpdateSupplierRateDto CreateUpdateSupplierRateDto(decimal rate = 175.00m)
        {
            return new UpdateSupplierRateDto
            {
                Rate = rate,
                RateStartDate = DateTime.Today.AddDays(1),
                RateEndDate = DateTime.Today.AddDays(61)
            };
        }

        public static OverlappingRateDto CreateOverlappingRateDto(int supplierId = 1, string supplierName = "Test Supplier")
        {
            return new OverlappingRateDto
            {
                SupplierId = supplierId,
                SupplierName = supplierName,
                OverlappingRates = new List<SupplierRateDto>
                {
                    CreateSupplierRateDto(1, supplierId, 100.00m),
                    CreateSupplierRateDto(2, supplierId, 120.00m)
                },
                OverlapReason = "Overlapping periods detected"
            };
        }

        public static List<Supplier> CreateSuppliersWithOverlappingRates()
        {
            var supplier = CreateSupplier(1, "Supplier with Overlaps");
            supplier.SupplierRates = new List<SupplierRate>
            {
                new SupplierRate
                {
                    SupplierRateId = 1,
                    SupplierId = 1,
                    Rate = 100.00m,
                    RateStartDate = DateTime.Today,
                    RateEndDate = DateTime.Today.AddDays(15),
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now
                },
                new SupplierRate
                {
                    SupplierRateId = 2,
                    SupplierId = 1,
                    Rate = 120.00m,
                    RateStartDate = DateTime.Today.AddDays(10),
                    RateEndDate = DateTime.Today.AddDays(25),
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now
                }
            };

            return new List<Supplier> { supplier };
        }

        public static List<Supplier> CreateSuppliersWithoutOverlappingRates()
        {
            var supplier = CreateSupplier(1, "Supplier without Overlaps");
            supplier.SupplierRates = new List<SupplierRate>
            {
                new SupplierRate
                {
                    SupplierRateId = 1,
                    SupplierId = 1,
                    Rate = 100.00m,
                    RateStartDate = DateTime.Today,
                    RateEndDate = DateTime.Today.AddDays(10),
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now
                },
                new SupplierRate
                {
                    SupplierRateId = 2,
                    SupplierId = 1,
                    Rate = 120.00m,
                    RateStartDate = DateTime.Today.AddDays(15),
                    RateEndDate = DateTime.Today.AddDays(25),
                    CreatedByUser = "TestUser",
                    CreatedOn = DateTime.Now
                }
            };

            return new List<Supplier> { supplier };
        }
    }
}
