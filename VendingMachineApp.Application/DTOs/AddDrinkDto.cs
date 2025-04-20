namespace VendingMachineApp.Application.DTOs
{
    public class AddDrinkDto 
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; } 
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int BrandId { get; set; } 
    }
}
