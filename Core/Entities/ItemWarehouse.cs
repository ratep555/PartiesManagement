namespace Core.Entities
{
    public class ItemWarehouse
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public int StockQuantity { get; set; }
    }
}