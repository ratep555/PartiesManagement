
namespace Core.Entities
{
    public class CountryShippingOption : BaseEntity
    {
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int ShippingOptionId { get; set; }
        public ShippingOption ShippingOption { get; set; }
        
        public decimal Price { get; set; }
    }
}