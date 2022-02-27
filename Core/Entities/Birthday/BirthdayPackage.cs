using System;
using System.Collections.Generic;
using Core.Entities.Birthday;

namespace Core.Entities
{
    public class BirthdayPackage : BaseEntity
    {
        public string PackageName { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public decimal Price { get; set; }
        public decimal AdditionalBillingPerParticipant { get; set; }
        public int Duration { get; set; }
        public string Picture { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public bool? HasDiscountsApplied { get; set; }


        public ICollection<BirthdayPackageService> BirthdayPackageServices { get; set; }
        public ICollection<BirthdayPackageDiscount> BirthdayPackageDiscounts { get; set; }
        public ICollection<Birthday1> Birthdays { get; set; }

    }
}