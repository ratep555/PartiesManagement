namespace Core.Entities
{
    public class Client : BaseEntity
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        
        public int CountryId { get; set; }
        public Country Country { get; set; }

    }
}