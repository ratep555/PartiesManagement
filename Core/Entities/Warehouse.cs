namespace Core.Entities
{
    public class Warehouse : BaseEntity
    {
        public string WarehouseName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        
        public int CountryId { get; set; }
        public Country Country { get; set; }

    }
}