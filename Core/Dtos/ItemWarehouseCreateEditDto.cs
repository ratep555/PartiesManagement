namespace Core.Dtos
{
    public class ItemWarehouseCreateEditDto
    {
        public int ItemId { get; set; }
        public int WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
    }
}