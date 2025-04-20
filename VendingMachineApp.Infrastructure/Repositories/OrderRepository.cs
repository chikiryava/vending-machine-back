using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;
using VendingMachineApp.Infrastructure.Persistence;

namespace VendingMachineApp.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _orderContext;
        public OrderRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<Order> AddAsync(Order order)
        {
            _orderContext.Orders.Add(order);
            await _orderContext.SaveChangesAsync();
            return order; 
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _orderContext.Orders.Include(o => o.Items).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orderToDelete = await _orderContext.Orders.FindAsync(id);
            if (orderToDelete == null)
                return false;
            _orderContext.Orders.Remove(orderToDelete);
            await _orderContext.SaveChangesAsync();
            return true;
        }
    }
}
