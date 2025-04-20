using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;
using VendingMachineApp.Infrastructure.Persistence;

namespace VendingMachineApp.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly InventoryContext _inventoryContext;
        public BrandRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _inventoryContext.Brands.ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _inventoryContext.Brands.Include(b => b.Drinks).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Brand> AddAsync(Brand brand)
        {
            _inventoryContext.Brands.Add(brand);
            await _inventoryContext.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> UpdateAsync(Brand brand)
        {
            var brandToUpdate = await _inventoryContext.Brands.FindAsync(brand.Id);
            if (brandToUpdate == null)
                return false;
            brandToUpdate.Name = brand.Name;
            await _inventoryContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brandToDelete = await _inventoryContext.Brands.FindAsync(id);
            if (brandToDelete == null)
                return false;
            _inventoryContext.Brands.Remove(brandToDelete);
            await _inventoryContext.SaveChangesAsync();
            return true; 
        }
    }
}
