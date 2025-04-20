using Microsoft.AspNetCore.Mvc;
using VendingMachineApp.Application.DTOs;
using VendingMachineApp.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;
    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(CreateOrderDto dto)
    {
        var res = await _orderService.CreateAsync(dto);

        if (res.Message.Contains("Недостаточно") || res.Message.Contains("Невозможно"))
            return BadRequest(res);

        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        return Ok(await _orderService.GetAllAsync());
    }

    [HttpGet("{id}")] 
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpDelete("{id}")] 
    public async Task<IActionResult> DeleteOrder(int id)
    {
        if (!await _orderService.DeleteAsync(id))
            return NotFound();

        return NoContent();
    }
}
