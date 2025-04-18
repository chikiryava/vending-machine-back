using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WendingApp.Models;

namespace VendingMachineApp.Data
{
    public class VendingMachineContext : DbContext
    {
        public VendingMachineContext(DbContextOptions<VendingMachineContext> options) : base(options) { }

        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
