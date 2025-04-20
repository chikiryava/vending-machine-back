using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;
using VendingMachineApp.Infrastructure.Persistence;

namespace VendingMachineApp.Infrastructure.Repositories
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly InventoryContext _inventoryContext;
        public DrinkRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public async Task<List<Drink>> GetAllAsync()
        {
            return await _inventoryContext.Drinks.Include(d => d.Brand).ToListAsync();
        }

        public async Task<Drink?> GetByIdAsync(int id)
        {
            return await _inventoryContext.Drinks.FindAsync(id);
        }

        public async Task<List<string>> GetNamesAsync()
        {
            return await _inventoryContext.Drinks.Select(d => d.Name.ToLower()).ToListAsync();
        }

        public async Task AddAsync(Drink drink) 
        {
            _inventoryContext.Drinks.Add(drink);
            await _inventoryContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Drink> drinks) 
        {
            _inventoryContext.Drinks.AddRange(drinks);
            await _inventoryContext.SaveChangesAsync(); 
        }

        public async Task<bool> UpdateAsync(Drink drink)
        {
            if (!await _inventoryContext.Drinks.AnyAsync(d => d.Id == drink.Id))
                return false;
            _inventoryContext.Drinks.Update(drink);
            await _inventoryContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var drinkToDelete = await _inventoryContext.Drinks.FindAsync(id);
            if (drinkToDelete == null)
                return false;
            _inventoryContext.Drinks.Remove(drinkToDelete);
            await _inventoryContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _inventoryContext.Drinks.AnyAsync(d => d.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Drink>> GetFilteredDrinksAsync(int? maxPrice = null, int? brandId = null)
        {
            var drinks = await GetAllAsync();
            if(brandId.HasValue)
                drinks = drinks.Where(d=>d.BrandId == brandId).ToList();
            if (maxPrice.HasValue)
                drinks = drinks.Where(d => d.Price <= maxPrice).ToList();
            return drinks;
        }
    }
}
