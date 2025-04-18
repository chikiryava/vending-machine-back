using Microsoft.AspNetCore.Mvc;
using VendingMachineApp.Interfaces;
using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]        
        public async Task<IActionResult> CreateOrderAndPay([FromBody] CreateOrderDto createOrderDto)
        {
            var result = await _orderService.CreateOrderAndProcessPaymentAsync(createOrderDto);
            if (result.Message.Contains("Недостаточно") || result.Message.Contains("Невозможно"))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}