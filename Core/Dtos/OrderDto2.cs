namespace Core.Dtos
{
    public class OrderDto2
    {
        public string BasketId { get; set; }
        public int ShippingOptionId { get; set; }
        public int PaymentOptionId { get; set; }
        public ShippingAddressDto1 ShippingAddress { get; set; }
    }
}