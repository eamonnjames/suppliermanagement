using SupplierManagement.Core.DTOs;
using SupplierManagement.Core.Entities;

namespace SupplierManagement.Core.Interfaces
{
    public interface ISupplierService
    {
        // Exercise 2 APIs
        Task<IEnumerable<SupplierWithRatesDto>> GetAllSuppliersWithRatesAsync();
        Task<IEnumerable<OverlappingRateDto>> GetOverlappingSuppliersAndRatesAsync(int? supplierId = null);

        // Exercise 1 CRUD Operations for Suppliers
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto?> GetSupplierByIdAsync(int id);
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto createSupplierDto);
        Task<SupplierDto?> UpdateSupplierAsync(int id, UpdateSupplierDto updateSupplierDto);
        Task<bool> DeleteSupplierAsync(int id);

        // Exercise 1 CRUD Operations for Supplier Rates
        Task<IEnumerable<SupplierRateDto>> GetRatesBySupplierIdAsync(int supplierId);
        Task<SupplierRateDto?> GetRateByIdAsync(int id);
        Task<SupplierRateDto> CreateRateAsync(CreateSupplierRateDto createRateDto);
        Task<SupplierRateDto?> UpdateRateAsync(int id, UpdateSupplierRateDto updateRateDto);
        Task<bool> DeleteRateAsync(int id);
    }
}
