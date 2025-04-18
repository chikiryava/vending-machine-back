using System;
using VendingMachineApp.Data;
using VendingMachineApp.Interfaces;
using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Services
{
    public class DrinkValidateService : IDrinkValidateService
    {
        private readonly IDrinkService _context;

        public DrinkValidateService(IDrinkService context)
        {
            _context = context;
        }

        public async Task<(List<Drink> ValidDrinks, List<string> Errors)> ValidateAndMapAsync(IEnumerable<DrinkImportDto> importItems)
        {
            var validDrinks = new List<Drink>();
            var errors = new List<string>();

            var existingNames = await _context.GetDrinkNamesAsync();

            foreach (var item in importItems)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    errors.Add("Имя напитка не может быть пустым.");
                    continue;
                }

                if (item.Price < 0)
                {
                    errors.Add($"Неверная цена у товара '{item.Name}'");
                    continue;
                }

                if (item.Quantity < 0)
                {
                    errors.Add($"Неверное количество у товара '{item.Name}'");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.ImageUrl))
                {
                    errors.Add($"URL изображения не указан у товара '{item.Name}'");
                    continue;
                }

                if (existingNames.Contains(item.Name.ToLower()))
                {
                    errors.Add($"Товар с именем '{item.Name}' уже существует");
                    continue;
                }

                validDrinks.Add(new Drink
                {
                    Name = item.Name.Trim(),
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl.Trim(),
                    BrandId = item.BrandId
                });
            }

            return (validDrinks, errors);
        }

        public Task<bool> IsDrinkValidAsync(Drink drink)
        {
            bool isValid = !string.IsNullOrWhiteSpace(drink.Name) &&
                           !string.IsNullOrWhiteSpace(drink.ImageUrl) &&
                           drink.Price >= 0 &&                           
                           drink.Quantity >= 0 &&
                           drink.BrandId > 0;

            return Task.FromResult(isValid);
        }
    }

}
