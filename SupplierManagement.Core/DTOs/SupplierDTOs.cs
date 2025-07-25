namespace SupplierManagement.Core.DTOs
{
    // DTOs for Exercise 2 APIs
    public class SupplierWithRatesDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public List<SupplierRateDto> Rates { get; set; } = new();
    }

    public class SupplierRateDto
    {
        public int SupplierRateId { get; set; }
        public int SupplierId { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateStartDate { get; set; }
        public DateTime? RateEndDate { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }

    public class OverlappingRateDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<SupplierRateDto> OverlappingRates { get; set; } = new();
        public string OverlapReason { get; set; } = string.Empty;
    }

    // DTOs for Exercise 1 CRUD Operations
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }

    public class CreateSupplierDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
    }

    public class UpdateSupplierDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public class CreateSupplierRateDto
    {
        public int SupplierId { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateStartDate { get; set; }
        public DateTime? RateEndDate { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
    }

    public class UpdateSupplierRateDto
    {
        public decimal Rate { get; set; }
        public DateTime RateStartDate { get; set; }
        public DateTime? RateEndDate { get; set; }
    }
}
