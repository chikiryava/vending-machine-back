using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Interfaces
{
    public interface IOrderService
    {
        Task<OrderPaymentResultDto> CreateOrderAndProcessPaymentAsync(CreateOrderDto createOrderDto);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task DeleteOrderAsync(int id);
    }
}
