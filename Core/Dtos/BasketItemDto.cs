namespace Core.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Picture { get; set; }
        public int? StockQuantity { get; set; }

    }
}