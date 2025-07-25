using System.ComponentModel.DataAnnotations;

namespace SupplierManagement.Core.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(450)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(450)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(450)]
        public string CreatedByUser { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; }

        // Navigation property
        public virtual ICollection<SupplierRate> SupplierRates { get; set; } = new List<SupplierRate>();
    }
}
