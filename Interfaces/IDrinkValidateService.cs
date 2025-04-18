using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Interfaces
{
    public interface IDrinkValidateService
    {
        Task<(List<Drink> ValidDrinks, List<string> Errors)> ValidateAndMapAsync(IEnumerable<DrinkImportDto> importItems);
        Task<bool> IsDrinkValidAsync(Drink drink);
    }

}
