using VendingMachineApp.Application.DTOs;
using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Application.Services
{
    public class DrinkValidateService
    {
        private readonly DrinkService _drinkService;
        public DrinkValidateService(DrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        public async Task<(List<Drink> Valid, List<string> Errors)> ValidateAndMapAsync(IEnumerable<DrinkImportDto> drinks)
        {
            var valid = new List<Drink>();
            var errors = new List<string>();
            var existing = await _drinkService.GetNamesAsync();

            foreach (var drink in drinks)
            {
                if (string.IsNullOrWhiteSpace(drink.Name))
                {
                    errors.Add("Имя пустое");
                    continue;
                }

                if (drink.Price < 0)
                {
                    errors.Add($"Цена неверна: {drink.Name}");
                    continue;
                }

                if (drink.Quantity < 0)
                {
                    errors.Add($"Кол-во неверно: {drink.Name}");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(drink.ImageUrl))
                {
                    errors.Add($"Нет URL: {drink.Name}");
                    continue;
                }

                if (existing.Contains(drink.Name.ToLower()))
                {
                    errors.Add($"Уже есть: {drink.Name}");
                    continue;
                }

                valid.Add(new Drink
                {
                    Name = drink.Name.Trim(),
                    Price = drink.Price,
                    Quantity = drink.Quantity,
                    ImageUrl = drink.ImageUrl.Trim(),
                    BrandId = drink.BrandId
                });
            }
            return (valid, errors);
        }

        public async Task<bool> IsValidAsync(Drink drink)
        {
            bool isValid = !string.IsNullOrWhiteSpace(drink.Name)
                      && !string.IsNullOrWhiteSpace(drink.ImageUrl)
                      && drink.Price >= 0
                      && drink.Quantity >= 0
                      && drink.BrandId > 0;
            return await Task.FromResult(isValid);
        }
    }
}
