using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Infrastructure.Persistence
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> opts) : base(opts) {}

        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Drink> Drinks => Set<Drink>();
        public DbSet<Coin> Coins => Set<Coin>();
    }
}
