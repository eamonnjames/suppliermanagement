using Microsoft.EntityFrameworkCore;
using SupplierManagement.Core.Entities;
using SupplierManagement.Core.Interfaces;
using SupplierManagement.Infrastructure.Data;

namespace SupplierManagement.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SupplierDbContext _context;

        public SupplierRepository(SupplierDbContext context)
        {
            _context = context;
        }

        // Exercise 2 methods
        public async Task<IEnumerable<Supplier>> GetAllSuppliersWithRatesAsync()
        {
            return await _context.Suppliers
                .Include(s => s.SupplierRates)
                .ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersWithOverlappingRatesAsync(int? supplierId = null)
        {
            var query = _context.Suppliers.Include(s => s.SupplierRates);

            if (supplierId.HasValue)
            {
                return await query.Where(s => s.SupplierId == supplierId.Value).ToListAsync();
            }

            return await query.ToListAsync();
        }

        // Exercise 1 CRUD methods for Suppliers
        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier?> GetSupplierByIdWithRatesAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.SupplierRates)
                .FirstOrDefaultAsync(s => s.SupplierId == id);
        }

        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier?> UpdateSupplierAsync(Supplier supplier)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(supplier.SupplierId);
            if (existingSupplier == null)
                return null;

            existingSupplier.Name = supplier.Name;
            existingSupplier.Address = supplier.Address;

            await _context.SaveChangesAsync();
            return existingSupplier;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.SupplierRates)
                .FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier == null)
                return false;

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        // Exercise 1 CRUD methods for Supplier Rates
        public async Task<IEnumerable<SupplierRate>> GetRatesBySupplierIdAsync(int supplierId)
        {
            return await _context.SupplierRates
                .Where(r => r.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<SupplierRate?> GetRateByIdAsync(int id)
        {
            return await _context.SupplierRates.FindAsync(id);
        }

        public async Task<SupplierRate> CreateRateAsync(SupplierRate rate)
        {
            _context.SupplierRates.Add(rate);
            await _context.SaveChangesAsync();
            return rate;
        }

        public async Task<SupplierRate?> UpdateRateAsync(SupplierRate rate)
        {
            var existingRate = await _context.SupplierRates.FindAsync(rate.SupplierRateId);
            if (existingRate == null)
                return null;

            existingRate.Rate = rate.Rate;
            existingRate.RateStartDate = rate.RateStartDate;
            existingRate.RateEndDate = rate.RateEndDate;

            await _context.SaveChangesAsync();
            return existingRate;
        }

        public async Task<bool> DeleteRateAsync(int id)
        {
            var rate = await _context.SupplierRates.FindAsync(id);
            if (rate == null)
                return false;

            _context.SupplierRates.Remove(rate);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
