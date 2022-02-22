using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class BirthdayPackage : BaseEntity
    {
        public string PackageName { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Picture { get; set; }
      //  public string Remarks { get; set; }

        public ICollection<BirthdayPackageService> BirthdayPackageServices { get; set; }
        public ICollection<Birthday> Birthdays { get; set; }

    }
}