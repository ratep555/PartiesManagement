using System.Collections.Generic;
using Core.Entities;

namespace Core.Dtos.Birthday
{
    public class BirthdayPackageDto
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Picture { get; set; }

        public ICollection<ServiceIncludedDto> ServicesIncluded { get; set; }
        public ICollection<BirthdayDto> Birthdays { get; set; }
    }
}