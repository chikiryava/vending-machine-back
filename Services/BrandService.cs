using Microsoft.EntityFrameworkCore;
using System;
using VendingMachineApp.Data;
using VendingMachineApp.Interfaces;
using WendingApp.Models;

namespace VendingMachineApp.Services
{
    public class BrandService : IBrandService
    {
        private readonly VendingMachineContext _context;

        public BrandService(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands
                .ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _context.Brands
                .Include(b => b.Drinks)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Brand> CreateBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            var existingBrand = await _context.Brands.FindAsync(brand.Id);
            if (existingBrand == null)
                return false;

            existingBrand.Name = brand.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return false;

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
