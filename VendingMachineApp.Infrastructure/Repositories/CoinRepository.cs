using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;
using VendingMachineApp.Infrastructure.Persistence;

namespace VendingMachineApp.Infrastructure.Repositories
{
    public class CoinRepository : ICoinRepository
    {
        private readonly InventoryContext _inventoryContext;
        public CoinRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public async Task<List<Coin>> GetAllAsync()
        {
            return await _inventoryContext.Coins.ToListAsync();
        }

        public async Task<Coin?> GetByIdAsync(int id)
        {
            return await _inventoryContext.Coins.FindAsync(id);
        }


        public async Task<Coin?> GetByNominalAsync(int nominal)
        {
            return await _inventoryContext.Coins.Where(c => c.Nominal == nominal).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateQuantityAsync(int id, int newQuantity)
        {
            var coinToUpdate = await _inventoryContext.Coins.FindAsync(id);
            if (coinToUpdate == null)
                return false;
            coinToUpdate.Quantity = newQuantity; 
            await _inventoryContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRangeQuantitiesAsync(IEnumerable<Coin> coinsToUpdate)
        {
            var updateMap = coinsToUpdate
                .ToDictionary(c => c.Nominal, c => c.Quantity);

            var nominalsToUpdate = updateMap.Keys.ToList();
            var coinsInDb = await _inventoryContext.Coins
                .Where(c => nominalsToUpdate.Contains(c.Nominal))
                .ToListAsync();

            foreach (var coinEntity in coinsInDb)
            {
                coinEntity.Quantity = updateMap[coinEntity.Nominal];
            }

            await _inventoryContext.SaveChangesAsync();

            return true;
        }

    }
}
