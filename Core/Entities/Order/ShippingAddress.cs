namespace Core.Entities.Order
{
    public class ShippingAddress
    {
        public ShippingAddress()
        {
            
        }
        public ShippingAddress(string firstName, string lastName, 
            string street, string city, int countryId)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            CountryId = countryId;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
    }
}