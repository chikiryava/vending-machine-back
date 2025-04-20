using Microsoft.AspNetCore.Mvc;
using VendingMachineApp.Application.Models.DTOs;
using VendingMachineApp.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class CoinsController : ControllerBase
{
    private readonly CoinService _coinService;
    public CoinsController(CoinService coinService)
    {
        _coinService = coinService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCoins()
    {
        return Ok(await _coinService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoinById(int id)
    {
        var coin = await _coinService.GetByIdAsync(id);

        if (coin == null)
            return NotFound();

        return Ok(coin);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCoinQuantity(int id, UpdateCoinDto updateCoinDto)
    {
        var coin = await _coinService.GetByIdAsync(updateCoinDto.Id);

        if(coin==null)
            return BadRequest("Монеты с таким Id не существует");

        if (updateCoinDto.Quantity < 0)
            return BadRequest("Количество меньше нуля");

        if (!await _coinService.UpdateQuantityAsync(id, updateCoinDto.Quantity)) 
            return NotFound();

        return NoContent();
    }
}
