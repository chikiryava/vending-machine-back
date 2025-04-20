using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Core.Interfaces
{
    public interface ICoinRepository
    {
        Task<List<Coin>> GetAllAsync();
        Task<Coin?> GetByIdAsync(int id);
        Task<bool> UpdateQuantityAsync(int id, int newQuantity);
        Task<Coin?> GetByNominalAsync(int nominal);
        Task<bool> UpdateRangeQuantitiesAsync(IEnumerable<Coin> coins);
    }
}
