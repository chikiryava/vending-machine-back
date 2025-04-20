using Microsoft.AspNetCore.Mvc;
using VendingMachineApp.Application.Services;
using VendingMachineApp.Core.Entities;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly BrandService _brandService;
    public BrandsController(BrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBrands()
    {
        return Ok(await _brandService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandById(int id)
    {
        var brand = await _brandService.GetByIdAsync(id);

        if (brand == null)
            return NotFound();

        return Ok(brand);
    }

    [HttpPost]
    public async Task<IActionResult> AddBrand(Brand brand)
    {
        var newBrand = await _brandService.CreateAsync(brand);
        return CreatedAtAction(nameof(GetBrandById), new { id = newBrand.Id }, newBrand);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand(int id, Brand brand)
    {
        if (id != brand.Id)
            return BadRequest();

        if (!await _brandService.UpdateAsync(brand)) 
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        if (!await _brandService.DeleteAsync(id))
            return NotFound();

        return NoContent();
    }
}
