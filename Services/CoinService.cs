using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Data;
using VendingMachineApp.Interfaces;
using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Services
{   

    public class CoinService : ICoinService
    {
        private readonly VendingMachineContext _context;

        public CoinService(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coin>> GetAllCoinsAsync()
        {
            return await _context.Coins.ToListAsync();
        }

        public async Task<Coin?> GetCoinByIdAsync(int id)
        {
            return await _context.Coins.FindAsync(id);
        }

        public async Task<bool> UpdateCoinQuantityAsync(UpdateCoinDto coin)
        {
            Coin coinToUpdate = await GetCoinByIdAsync(coin.Id);
            if(coinToUpdate == null)
            {
                return false;
            }
            coinToUpdate.Quantity = coin.Quantity;
            await _context.SaveChangesAsync();
            return true;

          
        }

        public Task<bool> IsCoinValidAsync(Coin coin)
        {
            bool isValid = coin.Nominal > 0 && coin.Quantity >= 0;
            return Task.FromResult(isValid);
        }
    }

}
