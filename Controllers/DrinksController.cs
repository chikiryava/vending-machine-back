using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using VendingMachineApp.Interfaces;
using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrinksController : ControllerBase
    {
        private readonly IDrinkService _drinkService;
        private readonly IDrinkValidateService _drinkValidateService;

        public DrinksController(IDrinkService drinkService, IDrinkValidateService drinkValidateService)
        {
            _drinkService = drinkService;
            _drinkValidateService = drinkValidateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrinks()
        {
            var drinks = await _drinkService.GetAllDrinksAsync();
            return Ok(drinks);
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetDrinkNames()
        {
            var names = await _drinkService.GetDrinkNamesAsync();
            return Ok(names);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrinkById(int id)
        {
            var drink = await _drinkService.GetDrinkByIdAsync(id);
            if (drink == null)
                return NotFound($"Напиток с Id {id} не найден");

            return Ok(drink);
        }

        [HttpPost]
        public async Task<IActionResult> AddDrink([FromBody] AddDrinkDto drinkDto)
        {
            if (drinkDto == null)
                return BadRequest("Информация о напитке null");

            var drinkToAdd = new Drink
            {
                Name = drinkDto.Name,
                Price = drinkDto.Price,
                Quantity = drinkDto.Quantity,
                BrandId = drinkDto.BrandId,
                ImageUrl = drinkDto.ImageUrl
            };

            var isValid = await _drinkValidateService.IsDrinkValidAsync(drinkToAdd);
            if (!isValid)
                return BadRequest("Данные напитка некорректны");

            try
            {
                await _drinkService.AddDrinkAsync(drinkToAdd);
                return CreatedAtAction(nameof(GetDrinkById), new { id = drinkToAdd.Id }, drinkToAdd);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrink(int id, [FromBody] UpdateDrinkDto updatedDrinkDto)
        {
            if (updatedDrinkDto == null || updatedDrinkDto.Id != id)
                return BadRequest("Информация о напитке отсутствует или неверный ID");

            var updatedDrink = new Drink
            {
                Id = id,
                Name = updatedDrinkDto.Name,
                Price = updatedDrinkDto.Price,
                Quantity = updatedDrinkDto.Quantity,
                ImageUrl = updatedDrinkDto.ImageUrl,
                BrandId = updatedDrinkDto.BrandId
            };

            var isValid = await _drinkValidateService.IsDrinkValidAsync(updatedDrink);
            if (!isValid)
                return BadRequest("Данные напитка некорректны");

            try
            {
                await _drinkService.UpdateDrinkAsync(updatedDrink);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrink(int id)
        {
            try
            {
                await _drinkService.DeleteDrinkAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("ImportDrinks")]
        public async Task<IActionResult> ImportDrinks(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран или пуст.");

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var importItems = new List<DrinkImportDto>();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                importItems.Add(new DrinkImportDto
                {
                    Name = worksheet.Cells[row, 1].GetValue<string>()?.Trim() ?? "",
                    Price = int.TryParse(worksheet.Cells[row, 2].GetValue<string>(), out var price) ? price : -1,
                    Quantity = int.TryParse(worksheet.Cells[row, 3].GetValue<string>(), out var qty) ? qty : -1,
                    ImageUrl = worksheet.Cells[row, 4].GetValue<string>() ?? "",
                    BrandId = int.TryParse(worksheet.Cells[row, 5].GetValue<string>(), out var brandId) ? brandId : 0
                });
            }

            var (validDrinks, errors) = await _drinkValidateService.ValidateAndMapAsync(importItems);

            try
            {
                if (validDrinks.Any())
                    await _drinkService.AddRangeDrinksAsync(validDrinks);

                return Ok(new
                {
                    ImportedCount = validDrinks.Count,
                    Errors = errors
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
