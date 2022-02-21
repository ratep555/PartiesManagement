using NetTopologySuite.Geometries;

namespace Core.Entities
{
    public class Location : BaseEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public Point Located { get; set; }
        public string Picture { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}