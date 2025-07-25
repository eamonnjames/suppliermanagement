using SupplierManagement.Core.DTOs;
using SupplierManagement.Core.Entities;
using SupplierManagement.Core.Interfaces;

namespace SupplierManagement.Core.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<SupplierWithRatesDto>> GetAllSuppliersWithRatesAsync()
        {
            var suppliers = await _supplierRepository.GetAllSuppliersWithRatesAsync();

            return suppliers.Select(s => new SupplierWithRatesDto
            {
                SupplierId = s.SupplierId,
                Name = s.Name,
                Address = s.Address,
                CreatedByUser = s.CreatedByUser,
                CreatedOn = s.CreatedOn,
                Rates = s.SupplierRates.Select(r => new SupplierRateDto
                {
                    SupplierRateId = r.SupplierRateId,
                    SupplierId = r.SupplierId,
                    Rate = r.Rate,
                    RateStartDate = r.RateStartDate,
                    RateEndDate = r.RateEndDate,
                    CreatedByUser = r.CreatedByUser,
                    CreatedOn = r.CreatedOn
                }).ToList()
            });
        }

        public async Task<IEnumerable<OverlappingRateDto>> GetOverlappingSuppliersAndRatesAsync(int? supplierId = null)
        {
            var suppliers = await _supplierRepository.GetAllSuppliersWithRatesAsync();

            if (supplierId.HasValue)
            {
                suppliers = suppliers.Where(s => s.SupplierId == supplierId.Value);
            }

            var overlappingResults = new List<OverlappingRateDto>();

            foreach (var supplier in suppliers)
            {
                var overlappingRates = FindOverlappingRates(supplier.SupplierRates.ToList());

                if (overlappingRates.Any())
                {
                    overlappingResults.Add(new OverlappingRateDto
                    {
                        SupplierId = supplier.SupplierId,
                        SupplierName = supplier.Name,
                        OverlappingRates = overlappingRates.Select(r => new SupplierRateDto
                        {
                            SupplierRateId = r.SupplierRateId,
                            SupplierId = r.SupplierId,
                            Rate = r.Rate,
                            RateStartDate = r.RateStartDate,
                            RateEndDate = r.RateEndDate,
                            CreatedByUser = r.CreatedByUser,
                            CreatedOn = r.CreatedOn
                        }).ToList(),
                        OverlapReason = "Date ranges have overlapping periods"
                    });
                }
            }

            return overlappingResults;
        }

        private List<SupplierRate> FindOverlappingRates(List<SupplierRate> rates)
        {
            var overlappingRates = new List<SupplierRate>();

            // Sort rates by start date
            var sortedRates = rates.OrderBy(r => r.RateStartDate).ToList();

            for (int i = 0; i < sortedRates.Count; i++)
            {
                for (int j = i + 1; j < sortedRates.Count; j++)
                {
                    var rate1 = sortedRates[i];
                    var rate2 = sortedRates[j];

                    if (DoRatesOverlap(rate1, rate2))
                    {
                        // Add both rates if they're not already in the list
                        if (!overlappingRates.Contains(rate1))
                            overlappingRates.Add(rate1);

                        if (!overlappingRates.Contains(rate2))
                            overlappingRates.Add(rate2);
                    }
                }
            }

            return overlappingRates;
        }

        private bool DoRatesOverlap(SupplierRate rate1, SupplierRate rate2)
        {
            // Get effective end dates (null means open-ended, use MaxValue)
            var rate1EndDate = rate1.RateEndDate ?? DateTime.MaxValue;
            var rate2EndDate = rate2.RateEndDate ?? DateTime.MaxValue;

            // Two date ranges overlap if:
            // rate1 starts before rate2 ends AND rate2 starts before rate1 ends
            return rate1.RateStartDate <= rate2EndDate && rate2.RateStartDate <= rate1EndDate;
        }

        // Exercise 1 CRUD Operations for Suppliers
        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllSuppliersAsync();
            return suppliers.Select(MapToSupplierDto);
        }

        public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
            return supplier != null ? MapToSupplierDto(supplier) : null;
        }

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto createSupplierDto)
        {
            var supplier = new Supplier
            {
                Name = createSupplierDto.Name,
                Address = createSupplierDto.Address,
                CreatedByUser = createSupplierDto.CreatedByUser,
                CreatedOn = DateTime.UtcNow
            };

            var createdSupplier = await _supplierRepository.CreateSupplierAsync(supplier);
            return MapToSupplierDto(createdSupplier);
        }

        public async Task<SupplierDto?> UpdateSupplierAsync(int id, UpdateSupplierDto updateSupplierDto)
        {
            var supplier = new Supplier
            {
                SupplierId = id,
                Name = updateSupplierDto.Name,
                Address = updateSupplierDto.Address
            };

            var updatedSupplier = await _supplierRepository.UpdateSupplierAsync(supplier);
            return updatedSupplier != null ? MapToSupplierDto(updatedSupplier) : null;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            return await _supplierRepository.DeleteSupplierAsync(id);
        }

        // Exercise 1 CRUD Operations for Supplier Rates
        public async Task<IEnumerable<SupplierRateDto>> GetRatesBySupplierIdAsync(int supplierId)
        {
            var rates = await _supplierRepository.GetRatesBySupplierIdAsync(supplierId);
            return rates.Select(MapToSupplierRateDto);
        }

        public async Task<SupplierRateDto?> GetRateByIdAsync(int id)
        {
            var rate = await _supplierRepository.GetRateByIdAsync(id);
            return rate != null ? MapToSupplierRateDto(rate) : null;
        }

        public async Task<SupplierRateDto> CreateRateAsync(CreateSupplierRateDto createRateDto)
        {
            var rate = new SupplierRate
            {
                SupplierId = createRateDto.SupplierId,
                Rate = createRateDto.Rate,
                RateStartDate = createRateDto.RateStartDate,
                RateEndDate = createRateDto.RateEndDate,
                CreatedByUser = createRateDto.CreatedByUser,
                CreatedOn = DateTime.UtcNow
            };

            var createdRate = await _supplierRepository.CreateRateAsync(rate);
            return MapToSupplierRateDto(createdRate);
        }

        public async Task<SupplierRateDto?> UpdateRateAsync(int id, UpdateSupplierRateDto updateRateDto)
        {
            var rate = new SupplierRate
            {
                SupplierRateId = id,
                Rate = updateRateDto.Rate,
                RateStartDate = updateRateDto.RateStartDate,
                RateEndDate = updateRateDto.RateEndDate
            };

            var updatedRate = await _supplierRepository.UpdateRateAsync(rate);
            return updatedRate != null ? MapToSupplierRateDto(updatedRate) : null;
        }

        public async Task<bool> DeleteRateAsync(int id)
        {
            return await _supplierRepository.DeleteRateAsync(id);
        }

        // Helper mapping methods
        private SupplierDto MapToSupplierDto(Supplier supplier)
        {
            return new SupplierDto
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                Address = supplier.Address,
                CreatedByUser = supplier.CreatedByUser,
                CreatedOn = supplier.CreatedOn
            };
        }

        private SupplierRateDto MapToSupplierRateDto(SupplierRate rate)
        {
            return new SupplierRateDto
            {
                SupplierRateId = rate.SupplierRateId,
                SupplierId = rate.SupplierId,
                Rate = rate.Rate,
                RateStartDate = rate.RateStartDate,
                RateEndDate = rate.RateEndDate,
                CreatedByUser = rate.CreatedByUser,
                CreatedOn = rate.CreatedOn
            };
        }
    }
}
