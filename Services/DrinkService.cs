using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Data;
using VendingMachineApp.Interfaces;
using WendingApp.Models;

namespace VendingMachineApp.Services
{
    public class DrinkService : IDrinkService
    {
        private readonly VendingMachineContext _context;

        public DrinkService(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<bool> IsDrinkExistsAsync(string name)
        {
            return await _context.Drinks.AnyAsync(d => d.Name.ToLower() == name.ToLower());
        }
        
        public async Task AddDrinkAsync(Drink drink)
        {
            _context.Drinks.Add(drink);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDrinkAsync(Drink drink)
        {
            _context.Drinks.Update(drink);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDrinkAsync(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Drink>> GetAllDrinksAsync()
        {
            return await _context.Drinks
                .Include(d => d.Brand)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDrinkNamesAsync()
        {
            return await _context.Drinks
                .Select(d => d.Name.ToLower())
                .ToListAsync();
        }

        public async Task AddRangeDrinksAsync(IEnumerable<Drink> drinks)
        {
            _context.Drinks.AddRange(drinks);
            await _context.SaveChangesAsync();
        }

        public async Task<Drink> GetDrinkByIdAsync(int id)
        {
            return await _context.Drinks.FindAsync(id);
        }
    }
}
