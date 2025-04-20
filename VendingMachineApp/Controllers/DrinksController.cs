using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using VendingMachineApp.Application.DTOs;
using VendingMachineApp.Application.Services;
using VendingMachineApp.Core.Entities;

[ApiController]
[Route("api/[controller]")]
public class DrinksController : ControllerBase
{
    private readonly DrinkService _drinkService;
    private readonly DrinkValidateService _drinkValidateService;
    public DrinksController(DrinkService drinkService, DrinkValidateService drinkValidateService)
    {
        _drinkService = drinkService;
        _drinkValidateService = drinkValidateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrinks()
    {
        return Ok(await _drinkService.GetAllAsync());
    }

    [HttpGet("names")]
    public async Task<IActionResult> GetAllNames()
    {
        return Ok(await _drinkService.GetNamesAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDrinkById(int id)
    {
        var drink = await _drinkService.GetByIdAsync(id);
        if (drink == null) 
            return NotFound();
        return Ok(drink);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredDrinks([FromQuery] DrinkFilterDto filter)
    {
        try
        {
            var drinks = await _drinkService.GetFilteredDrinksAsync(filter);
            if (!drinks.Any())
                return NotFound();
            return Ok(drinks);
        }
        catch(ArgumentOutOfRangeException ex)
        {
            return BadRequest("Минимальная цена должна быть больше нуля");
        }        
    }

    [HttpPost]
    public async Task<IActionResult> AddDrink(AddDrinkDto dto)
    {
        var drink = new Drink
        {
            Name = dto.Name,
            Price = dto.Price,
            Quantity = dto.Quantity,
            ImageUrl = dto.ImageUrl,
            BrandId = dto.BrandId
        };

        if (!await _drinkValidateService.IsValidAsync(drink))
            return BadRequest("Данные некорректны");

        if (await _drinkService.ExistsByNameAsync(drink.Name))
            return Conflict($"Напиток с названием {drink.Name} уже существует");

        await _drinkService.AddAsync(drink);
        return CreatedAtAction(nameof(GetDrinkById), new { id = drink.Id }, drink);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDrink(int id, UpdateDrinkDto dto)
    {
        if (id != dto.Id) 
            return BadRequest();
        var drink = new Drink
        {
            Id = id,
            Name = dto.Name,
            Price = dto.Price,
            Quantity = dto.Quantity,
            ImageUrl = dto.ImageUrl,
            BrandId = dto.BrandId
        };

        if (!await _drinkValidateService.IsValidAsync(drink))
            return BadRequest("Ошибка в данных");

        if (!await _drinkService.UpdateAsync(drink))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrink(int id)
    {
        if (!await _drinkService.DeleteAsync(id))
            return NotFound();
        return NoContent();
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
                await _drinkService.AddRangeAsync(validDrinks);

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
