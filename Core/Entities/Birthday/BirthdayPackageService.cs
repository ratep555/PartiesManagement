namespace Core.Entities
{
    public class BirthdayPackageService
    {
        public int BirthdayPackageId { get; set; }
        public BirthdayPackage BirthdayPackage { get; set; }

        public int ServiceIncludedId { get; set; }
        public ServiceIncluded ServiceIncluded { get; set; }

    }
}