namespace Core.Dtos
{
    public class ItemWarehouseDto
    {
        public int ItemId { get; set; }
        public int WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
        public string Item { get; set; }
        public string Warehouse { get; set; }
    }
}