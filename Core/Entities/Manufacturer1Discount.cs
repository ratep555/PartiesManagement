namespace Core.Entities
{
    public class Manufacturer1Discount
    {
        public int Manufacturer1Id { get; set; }
        public Manufacturer1 Manufacturer1 { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}