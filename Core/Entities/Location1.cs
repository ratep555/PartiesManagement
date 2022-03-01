using NetTopologySuite.Geometries;

namespace Core.Entities
{
    public class Location1 : BaseEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public Point Location { get; set; }
        public string Picture { get; set; }
        public string WorkingHours { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}