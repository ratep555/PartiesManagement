namespace Core.Entities
{
    public class ItemCategory
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}