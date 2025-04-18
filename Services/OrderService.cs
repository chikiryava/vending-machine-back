using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Data;
using VendingMachineApp.Interfaces;
using VendingMachineApp.Models.DTOs;
using WendingApp.Models;

namespace VendingMachineApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly VendingMachineContext _context;
        public OrderService(VendingMachineContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
                throw new InvalidOperationException("Заказ не найден.");

            _context.Orders.Remove(existingOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderPaymentResultDto> CreateOrderAndProcessPaymentAsync(CreateOrderDto createOrderDto)
        {
            var result = new OrderPaymentResultDto();
            var order = new Order();
            decimal orderTotal = 0;

            var drinkIds = createOrderDto.Items.Select(i => i.DrinkId).Distinct().ToList();

            var drinksInDb = await _context.Drinks
                .Where(d => drinkIds.Contains(d.Id))
                .ToDictionaryAsync(d => d.Id);

            foreach (var itemDto in createOrderDto.Items)
            {
                if (!drinksInDb.TryGetValue(itemDto.DrinkId, out var drink) || drink.Quantity < itemDto.Quantity)
                {
                    result.Message = $"Недостаточно товара для напитка ID {itemDto.DrinkId}";
                    return result;
                }

                drink.Quantity -= itemDto.Quantity;

                var orderItem = new OrderItem
                {
                    DrinkName = drink.Name,
                    Price = drink.Price,
                    Quantity = itemDto.Quantity
                };

                orderTotal += drink.Price * itemDto.Quantity;
                order.Items.Add(orderItem);
            }

            order.Total = orderTotal;
            await _context.Orders.AddAsync(order);

            decimal insertedAmount = createOrderDto.InsertedCoins.Sum(c => c.Nominal * c.Quantity);
            result.InsertedAmount = insertedAmount;
            result.OrderTotal = orderTotal;

            if (insertedAmount < orderTotal)
            {
                result.Message = "Недостаточно средств для оплаты";
                return result;
            }

            // Расчет сдачи
            decimal changeAmount = insertedAmount - orderTotal;
            result.ChangeAmount = changeAmount;

            var nominals = createOrderDto.InsertedCoins.Select(c => c.Nominal).Distinct().ToList();

            var coinsInDb = await _context.Coins
                .Where(c => nominals.Contains(c.Nominal))
                .ToDictionaryAsync(c => c.Nominal);

            // Обрабатываем внесённые монеты
            foreach (var coinDto in createOrderDto.InsertedCoins)
            {
                if (coinsInDb.TryGetValue(coinDto.Nominal, out var coinInDb))
                {
                    coinInDb.Quantity += coinDto.Quantity;
                }
            }

            // Вычисляем выдачу сдачи: пытаемся "разменять" сдачу на монеты, доступные в автомате
            var changeCoins = await ComputeChangeAsync(changeAmount);
            if (changeCoins == null)
            {
                result.Message = "Невозможно выдать нужную сдачу";
                return result;
            }
            result.ChangeCoins = changeCoins;

            await _context.SaveChangesAsync();

            result.Message = "Заказ и оплата прошли успешно";
            return result;
        }

        // Метод разбивки сдачи: пытаемся выдать сдачу, используя монеты, доступные в автомате
        private async Task<List<CoinDto>?> ComputeChangeAsync(decimal changeAmount)
        {
            var changeCoins = new List<CoinDto>();
            // Получаем монеты, отсортированные по убыванию номинала
            var availableCoins = await _context.Coins
                .OrderByDescending(c => c.Nominal)
                .ToListAsync();

            decimal remaining = changeAmount;
            foreach (var coin in availableCoins)
            {
                int count = 0;
                while (remaining >= coin.Nominal && coin.Quantity - count > 0)
                {
                    remaining -= coin.Nominal;
                    count++;
                }
                if (count > 0)
                {
                    changeCoins.Add(new CoinDto { Nominal = coin.Nominal, Quantity = count });
                }
            }

            if (remaining > 0)
            {
                return null;
            }


            var coinDict = availableCoins.ToDictionary(c => c.Nominal);

            foreach (var coinDto in changeCoins)
            {
                if (coinDict.TryGetValue(coinDto.Nominal, out var coin))
                {
                    coin.Quantity -= coinDto.Quantity;
                }
            }

            _context.Coins.UpdateRange(coinDict.Values);

            return changeCoins;
        }
    }
}
