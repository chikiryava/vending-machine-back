using WendingApp.Models;

namespace VendingMachineApp.Interfaces
{
    public interface IDrinkService
    {
        Task<bool> IsDrinkExistsAsync(string name);
        Task<IEnumerable<string>> GetDrinkNamesAsync();
        Task AddDrinkAsync(Drink drink);
        Task AddRangeDrinksAsync(IEnumerable<Drink> drinks);
        Task UpdateDrinkAsync(Drink drink);
        Task DeleteDrinkAsync(int id);
        Task<IEnumerable<Drink>> GetAllDrinksAsync();
        Task<Drink> GetDrinkByIdAsync(int id);
    }

}
