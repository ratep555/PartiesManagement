namespace Core.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int ShippingOptionId { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
    }
}