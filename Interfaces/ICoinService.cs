using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Interfaces
{
    public interface ICoinService
    {
        Task<IEnumerable<Coin>> GetAllCoinsAsync();
        Task<Coin?> GetCoinByIdAsync(int id);       
        Task<bool> UpdateCoinQuantityAsync(UpdateCoinDto coin);
        Task<bool> IsCoinValidAsync(Coin coin);
    }
}
