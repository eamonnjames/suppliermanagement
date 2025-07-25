using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierManagement.Core.Entities
{
    public class SupplierRate
    {
        public int SupplierRateId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [Required]
        public DateTime RateStartDate { get; set; }

        public DateTime? RateEndDate { get; set; }

        [Required]
        [MaxLength(450)]
        public string CreatedByUser { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; }

        // Navigation property
        public virtual Supplier Supplier { get; set; } = null!;
    }
}
