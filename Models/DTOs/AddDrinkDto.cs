namespace VendingMachineApp.Models.DTOs
{
    public class AddDrinkDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int BrandId { get; set; }
    }
}
