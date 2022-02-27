namespace Core.Dtos
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }        
        public string Picture { get; set; }
        public string Country { get; set; }
    }
}