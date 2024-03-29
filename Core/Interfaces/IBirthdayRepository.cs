using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos.Birthday;
using Core.Entities;
using Core.Entities.Blogs;
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
        Task<Location1> FindLocationById(int id);
        Task<List<Location1>> GetAllLocations(QueryParameters queryParameters);
        Task<int> GetCountForLocations();
        Task CreateLocation(Location1 location);
        Task UpdateLocation(Location1 location);
        Task<ApplicationUser> GetAdmin();
        Task CreateMessage(Message message);
        Task AddServiceIncluded(ServiceIncluded serviceIncluded);
        Task<ServiceIncluded> GetServiceIncludedById(int id);
        Task UpdateServiceIncluded(ServiceIncluded serviceIncluded);
        Task<List<ServiceIncluded>> GetAllServicesIncluded(QueryParameters queryParameters);
        Task<int> GetCountForServicesIncluded();
        Task AddBlog(Blog blog);
        Task<List<Blog>> GetAllBlogsForUser(int userId, QueryParameters queryParameters);  
        Task<int> GetCountForBlogsForUser(int userId);
        Task<List<Blog>> GetAllBlogs(QueryParameters queryParameters);
        Task<int> GetCountForBlogs();
        Task<Blog> GetBlogById(int id);
        Task UpdateBlog(Blog blog);
        Task AddBlogComment(BlogComment blogComment);
        Task UpdateBlogComment(BlogComment blogComment);
        Task<BlogComment> GetBlogCommentById(int id);
        Task<List<BlogComment>> GetAllBlogComments(int blogId);
        void DeleteBlogComment(BlogComment blogComment);
        Task<int> Complete();


    }
}






