using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos.Birthday;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IBirthdayRepository
    {
        // birthdays
        Task<List<Birthday1>> GetAllBirthdays(QueryParameters queryParameters);
        Task<int> GetCountForBirthdays();
        Task<Birthday1> FindBirthdayById(int id);
        Task AddBirthday(Birthday1 birthday);
        Task UpdateBirthday(Birthday1 birthday);

        // birthdaypackages
        Task<List<BirthdayPackage>> GetAllBirthdayPackages(QueryParameters queryParameters);
        Task<int> GetCountForBirthdayPackages();
        Task<BirthdayPackage> GetBirthdayPackageById(int id);
        Task<List<Location1>> GetLocations();
        Task<List<BirthdayPackage>> GetBirthdayPackages();
        Task AddBirthdayPackage(BirthdayPackage package);
        Task<List<ServiceIncluded>> GetServices();
        Task<List<ServiceIncluded>> GetNonSelectedServices(List<int> ids);
        Task UpdateBirthdayPackage(BirthdayPackage birthdayPackage);
        Task UpdateBirthdayPackageWithDiscount(BirthdayPackage birthdayPackage);
        Task ResetBirthdayPackageDiscountedPrice(BirthdayPackage birthdayPackage);
        Task ResetBirthdayPackageDiscountedPriceDueToDiscountExpiry(IEnumerable<BirthdayPackage> birthdayPackages);
        Task ResetBirthayPackageDiscountedPrice(Discount discount);
        Task UpdateBirthdayPackageWithDiscount(Discount discount);
        Task UpdateBirthdayPackageWithDiscount1(BirthdayPackage birthdayPackage);
        Task<decimal> DiscountSum(BirthdayPackage birthdayPackage);
        Task DiscountSum1(IEnumerable<BirthdayPackage> birthdayPackages,
            IEnumerable<BirthdayPackageDto> birthdayPackagesDto);

    }
}






