using VendingMachineApp.Application.DTOs;
using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;

namespace VendingMachineApp.Application.Services
{
    public class DrinkService
    {
        private readonly IDrinkRepository _drinkRepository;
        public DrinkService(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        public async Task<List<Drink>> GetAllAsync()
        {
            return await _drinkRepository.GetAllAsync();
        }

        public async Task<List<Drink>> GetFilteredDrinksAsync(DrinkFilterDto drinkFilterDto )
        {
            if(drinkFilterDto.MaxPrice<0)
                throw new ArgumentOutOfRangeException(nameof(drinkFilterDto.MaxPrice), "Макс. цена не может быть меньше нуля");
            return await _drinkRepository.GetFilteredDrinksAsync(drinkFilterDto.MaxPrice, drinkFilterDto.BrandId);
        }

        public async Task<Drink?> GetByIdAsync(int id)
        {
            return await _drinkRepository.GetByIdAsync(id);
        }

        public async Task<List<string>> GetNamesAsync()
        {
            return await _drinkRepository.GetNamesAsync();
        }

        public async Task AddAsync(Drink drink)
        {
            await _drinkRepository.AddAsync(drink);
        }

        public async Task AddRangeAsync(IEnumerable<Drink> drinks)
        {
            await _drinkRepository.AddRangeAsync(drinks);
        }

        public async Task<bool> UpdateAsync(Drink drink)
        {
            return await _drinkRepository.UpdateAsync(drink);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _drinkRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _drinkRepository.ExistsByNameAsync(name);
        }
    }
}
