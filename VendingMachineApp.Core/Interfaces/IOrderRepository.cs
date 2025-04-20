using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
