namespace VendingMachineApp.Models.DTOs
{
    public class CreateOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
        public List<CoinDto> InsertedCoins { get; set; } = new();
    }
}
