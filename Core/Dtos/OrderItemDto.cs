namespace Core.Dtos
{
    public class OrderItemDto
    {
         public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}