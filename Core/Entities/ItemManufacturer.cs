namespace Core.Entities
{
    public class ItemManufacturer
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}