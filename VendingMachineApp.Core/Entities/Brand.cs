using System.Text.Json.Serialization;

namespace VendingMachineApp.Core.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Drink> Drinks { get; set; } = new();
    }
}
