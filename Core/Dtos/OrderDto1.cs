namespace Core.Dtos
{
    public class OrderDto1
    {
        public string BasketId { get; set; }
        public int ShippingOptionId { get; set; }
        public ShippingAddressDto1 ShippingAddress { get; set; }
    }
}