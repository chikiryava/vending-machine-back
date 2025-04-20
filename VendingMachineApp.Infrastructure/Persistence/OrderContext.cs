using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> opts) : base(opts) {}

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    }
}
