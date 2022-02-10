namespace Core.Entities
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(BasketItemOrdered basketItemOrdered, decimal price, int quantity, string picture)
        {
            BasketItemOrdered = basketItemOrdered;
            Price = price;
            Quantity = quantity;
            Picture = picture;
        }

        public BasketItemOrdered BasketItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Picture { get; set; }
        public int MyProperty { get; set; }
    }
}