using VendingMachineApp.Application.DTOs;
using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;

namespace VendingMachineApp.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly DrinkService _drinkService;
        private readonly ICoinRepository _coinRepo;

        public OrderService(IOrderRepository orderRepo, DrinkService drinkService, ICoinRepository coinRepo)
        {
            _orderRepo = orderRepo;
            _drinkService = drinkService;
            _coinRepo = coinRepo;
        }

        public async Task<OrderPaymentResultDto> CreateAsync(CreateOrderDto createOrderDto)
        {
            var result = new OrderPaymentResultDto();
            var order = new Order();
            int totalAmount = 0;

            var requestedDrinkIds = createOrderDto.Items
                .Select(item => item.DrinkId)
                .Distinct()
                .ToList();

            var allDrinks = await _drinkService.GetAllAsync();
            var drinksById = allDrinks
                .Where(d => requestedDrinkIds.Contains(d.Id))
                .ToDictionary(d => d.Id);

            foreach (var itemDto in createOrderDto.Items)
            {
                if (!drinksById.TryGetValue(itemDto.DrinkId, out var drinkEntity)
                    || drinkEntity.Quantity < itemDto.Quantity)
                {
                    result.Message = $"Недостаточно напитка с ID {itemDto.DrinkId}";
                    return result;
                }

                drinkEntity.Quantity -= itemDto.Quantity;
                order.Items.Add(new OrderItem
                {
                    DrinkName = drinkEntity.Name,
                    Price = drinkEntity.Price,
                    Quantity = itemDto.Quantity
                });
                totalAmount += drinkEntity.Price * itemDto.Quantity;
            }

            order.Total = totalAmount;
            await _orderRepo.AddAsync(order);

            var totalInserted = createOrderDto.InsertedCoins
                .Sum(c => c.Nominal * c.Quantity);
            result.InsertedAmount = totalInserted;
            result.OrderTotal = totalAmount;

            if (totalInserted < totalAmount)
            {
                result.Message = "Недостаточно средств";
                return result;
            }

            var changeAmount = totalInserted - totalAmount;
            result.ChangeAmount = changeAmount;

            var allCoins = await _coinRepo.GetAllAsync();
            var coinStockByNominal = allCoins
                .ToDictionary(c => c.Nominal, c => c.Quantity);

            foreach (var inserted in createOrderDto.InsertedCoins)
            {
                if (coinStockByNominal.ContainsKey(inserted.Nominal))
                    coinStockByNominal[inserted.Nominal] += inserted.Quantity;
                else
                    coinStockByNominal[inserted.Nominal] = inserted.Quantity;
            }

            var changeCoins = new List<CoinDto>();
            decimal remaining = changeAmount;
            foreach (var nominal in coinStockByNominal.Keys.OrderByDescending(n => n))
            {
                int count = 0;
                while (remaining >= nominal && coinStockByNominal[nominal] - count > 0)
                {
                    remaining -= nominal;
                    count++;
                }
                if (count > 0)
                {
                    changeCoins.Add(new CoinDto { Nominal = nominal, Quantity = count });
                }
            }

            if (remaining > 0)
            {
                result.Message = "Невозможно выдать сдачу";
                return result;
            }

            result.ChangeCoins = changeCoins;

            foreach (var changeCoin in changeCoins)
            {
                coinStockByNominal[changeCoin.Nominal] -= changeCoin.Quantity;
            }

            var updatedCoinStocks = coinStockByNominal
                .Select(kvp => new Coin { Nominal = kvp.Key, Quantity = kvp.Value })
                .ToList();

            await _coinRepo.UpdateRangeQuantitiesAsync(updatedCoinStocks);

            result.Message = "Успех";
            return result;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _orderRepo.GetAllAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderRepo.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepo.DeleteAsync(id);
        }
    }
}
