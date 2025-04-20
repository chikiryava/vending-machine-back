using System.Text.Json.Serialization;

namespace VendingMachineApp.Core.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string DrinkName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
    }
}
