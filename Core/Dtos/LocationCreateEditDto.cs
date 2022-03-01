using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class LocationCreateEditDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        

        [Range(-90, 90)]
        public double Latitude { get; set; }


        [Range(-180, 180)]
        public double Longitude { get; set; }  

        public string Picture { get; set; }
        public string WorkingHours { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CountryId { get; set; }
    }
}