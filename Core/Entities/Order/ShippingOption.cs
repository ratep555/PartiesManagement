namespace Core.Entities
{
    public class ShippingOption : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TransitDays { get; set; }
        public decimal Price { get; set; }
    }
}