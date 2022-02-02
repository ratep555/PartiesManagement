namespace Core.Dtos
{
    public class ShippingOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TransitDays { get; set; }
        public decimal Price { get; set; }
    }
}