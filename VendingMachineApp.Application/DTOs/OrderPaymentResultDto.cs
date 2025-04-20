namespace VendingMachineApp.Application.DTOs
{
    public class OrderPaymentResultDto
    {
        public decimal OrderTotal { get; set; }
        public decimal InsertedAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public List<CoinDto> ChangeCoins { get; set; } = new(); public string Message { get; set; } = string.Empty;
    }
}
