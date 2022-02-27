namespace Core.Entities.Birthday
{
    public class BirthdayPackageDiscount
    {
        public int BirthdayPackageId { get; set; }
        public BirthdayPackage BirthdayPackage { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }

    }
}