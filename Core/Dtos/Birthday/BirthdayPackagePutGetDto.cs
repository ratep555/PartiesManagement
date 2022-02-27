using System.Collections.Generic;

namespace Core.Dtos.Birthday
{
    public class BirthdayPackagePutGetDto
    {
        public BirthdayPackageDto BirthdayPackage { get; set; }
        public IEnumerable<ServiceIncludedDto> SelectedServices { get; set; }
        public IEnumerable<ServiceIncludedDto> NonSelectedServices { get; set; }
        public IEnumerable<DiscountDto> SelectedDiscounts { get; set; }
        public IEnumerable<DiscountDto> NonSelectedDiscounts { get; set; }
    }
}