using System.Text.Json.Serialization;

namespace VendingMachineApp.Core.Entities
{
    public class Drink
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int BrandId { get; set; }
        [JsonIgnore]
        public Brand? Brand { get; set; }
    }
}
