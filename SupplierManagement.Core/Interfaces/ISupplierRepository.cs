using SupplierManagement.Core.Entities;

namespace SupplierManagement.Core.Interfaces
{
    public interface ISupplierRepository
    {
        // Exercise 2 methods
        Task<IEnumerable<Supplier>> GetAllSuppliersWithRatesAsync();
        Task<IEnumerable<Supplier>> GetSuppliersWithOverlappingRatesAsync(int? supplierId = null);

        // Exercise 1 CRUD methods for Suppliers
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<Supplier?> GetSupplierByIdWithRatesAsync(int id);
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task<Supplier?> UpdateSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(int id);

        // Exercise 1 CRUD methods for Supplier Rates
        Task<IEnumerable<SupplierRate>> GetRatesBySupplierIdAsync(int supplierId);
        Task<SupplierRate?> GetRateByIdAsync(int id);
        Task<SupplierRate> CreateRateAsync(SupplierRate rate);
        Task<SupplierRate?> UpdateRateAsync(SupplierRate rate);
        Task<bool> DeleteRateAsync(int id);
    }
}
