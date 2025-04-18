using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using VendingMachineApp.Interfaces;
using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public CoinsController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoins()
        {
            var coins = await _coinService.GetAllCoinsAsync();
            return Ok(coins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoinById(int id)
        {
            var coin = await _coinService.GetCoinByIdAsync(id);
            if (coin == null)
                return NotFound();

            return Ok(coin);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoinQuantity(int id, [FromBody] UpdateCoinDto coin)
        {
            if (id != coin.Id)
                return BadRequest("ID в URL и в теле запроса не совпадают.");
            if (coin.Quantity < 0)
                return BadRequest("Количество монет не может быть меньше нуля");
            var result = await _coinService.UpdateCoinQuantityAsync(coin);
            if (!result)
                return BadRequest("Монеты с таким ID не существует");

            return NoContent();
        }

    }
}
