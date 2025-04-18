using System.Text.Json.Serialization;

namespace WendingApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public string DrinkName { get; set; } = string.Empty;
        public decimal Price { get; set; } // Цена на момент заказа
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; } = null!;
    }
}
