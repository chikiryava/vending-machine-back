using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Core.Interfaces
{
    public interface IDrinkRepository
    {
        Task<List<Drink>> GetAllAsync();
        Task<Drink?> GetByIdAsync(int id);
        Task<List<string>> GetNamesAsync();
        Task<List<Drink>> GetFilteredDrinksAsync(int? minPrice = null, int? brandId = null);
        Task AddAsync(Drink drink);
        Task AddRangeAsync(IEnumerable<Drink> drinks);
        Task<bool> UpdateAsync(Drink drink);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
