using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.Birthday;
using Core.Entities;
using Core.Entities.Blogs;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class BirthdayRepository : IBirthdayRepository
    {
        private readonly PartiesContext _context;
        public BirthdayRepository(PartiesContext context)
        {
            _context = context;
        }
        
        // birthdays
        public async Task<List<Birthday1>> GetAllBirthdays(QueryParameters queryParameters)
        {
            IQueryable<Birthday1> birthdays = _context.Birthdays.Include(x => x.BirthdayPackage)
                .Include(x => x.Location)
                .Include(x => x.OrderStatus1)
                .AsQueryable().OrderBy(x => x.StartDateAndTime);

            if (queryParameters.HasQuery())
            {
                birthdays = birthdays.Where(t => t.ClientName.Contains(queryParameters.Query));
            }

             if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "all":
                        birthdays = birthdays.OrderBy(p => p.StartDateAndTime);
                        break;
                    case "pending":
                        birthdays = birthdays.Where(p => p.OrderStatus1Id == null);
                        break;
                    case "approved":
                        birthdays = birthdays.Where(p => p.OrderStatus1Id == 7);
                        break;
                    default:
                        birthdays = birthdays.OrderBy(p => p.Id);
                        break;
                }
            }    

            birthdays = birthdays.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await birthdays.ToListAsync();
        }

        public async Task<int> GetCountForBirthdays()
        {
            return await _context.Birthdays.CountAsync();
        }

        public async Task<Birthday1> FindBirthdayById(int id)
        {
            return await _context.Birthdays.Include(x => x.BirthdayPackage)
                .Include(x => x.Location).Include(x => x.OrderStatus1)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddBirthday(Birthday1 birthday)
        {
            var birthdayPackage = await _context.BirthdayPackages
                .FirstOrDefaultAsync(x => x.Id == birthday.BirthdayPackageId);
            
            int minutes = birthdayPackage.Duration;

            birthday.StartDateAndTime = birthday.StartDateAndTime.ToLocalTime();
            birthday.EndDateAndTime = birthday.StartDateAndTime.AddMinutes(minutes).ToLocalTime();
            birthday.Price = CalculateBirthdayPrice(birthday, birthdayPackage);

            _context.Birthdays.Add(birthday);

            await _context.SaveChangesAsync();
        }

        public decimal CalculateBirthdayPrice(Birthday1 birthday, BirthdayPackage birthdayPackage)
        {
            decimal price = 0;

            if (birthday.NumberOfGuests > birthdayPackage.NumberOfParticipants)
            {
                var leftover = birthday.NumberOfGuests - birthdayPackage.NumberOfParticipants;
                var additionalBilling = leftover * birthdayPackage.AdditionalBillingPerParticipant;

                price = birthdayPackage.Price + additionalBilling;
            }
            else
            {
                price = birthdayPackage.Price;
            }

            return price;
        }

        public async Task UpdateBirthday(Birthday1 birthday)
        {    
            _context.Entry(birthday).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }


        // birthdaypackages
        public async Task<List<BirthdayPackage>> GetAllBirthdayPackages(QueryParameters queryParameters)
        {
            IQueryable<BirthdayPackage> birthdayPackages = _context.BirthdayPackages
                .Include(x => x.BirthdayPackageServices).ThenInclude(x => x.ServiceIncluded)
                .AsQueryable().OrderBy(x => x.PackageName);

            if (queryParameters.HasQuery())
            {
                birthdayPackages = birthdayPackages.Where(t => t.PackageName.Contains(queryParameters.Query));
            }

            birthdayPackages = birthdayPackages.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await birthdayPackages.ToListAsync();
        }

        public async Task<int> GetCountForBirthdayPackages()
        {
            return await _context.BirthdayPackages.CountAsync();
        }

        public async Task<BirthdayPackage> GetBirthdayPackageById(int id)
        {
            return await _context.BirthdayPackages.Include(x => x.BirthdayPackageServices)
                .ThenInclude(x => x.ServiceIncluded).Include(x => x.BirthdayPackageDiscounts)
                .ThenInclude(x => x.Discount).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Location1>> GetLocations()
        {
            return await _context.Locations.Include(X => X.Country)
                .OrderBy(x => x.City).ToListAsync();
        }

        public async Task<List<BirthdayPackage>> GetBirthdayPackages()
        {
            return await _context.BirthdayPackages.OrderBy(x => x.PackageName).ToListAsync();
        }

        public async Task AddBirthdayPackage(BirthdayPackage package)
        {
            _context.BirthdayPackages.Add(package);

            await _context.SaveChangesAsync();
        }

        public async Task<List<ServiceIncluded>> GetServices()
        {
            return await _context.ServicesIncluded.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<ServiceIncluded>> GetNonSelectedServices(List<int> ids)
        {
            return await _context.ServicesIncluded.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task UpdateBirthdayPackage(BirthdayPackage birthdayPackage)
        {    
            _context.Entry(birthdayPackage).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        public async Task UpdateBirthdayPackageWithDiscount(BirthdayPackage birthdayPackage)
        {     
            var birthdayPackakgeDiscounts = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.BirthdayPackageId == birthdayPackage.Id).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackakgeDiscounts.Select(x => x.BirthdayPackageId);

            var list = await _context.BirthdayPackages.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

            foreach (var package in list)
            {
                var discountPercentage2 = await _context.BirthdayPackageDiscounts
                    .Where(x => x.BirthdayPackageId == birthdayPackage.Id 
                    && x.BirthdayPackageId == package.Id).FirstOrDefaultAsync();

                decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                var discountAmount = (discountPercentage1 / 100) * birthdayPackage.Price;
                
                if (discountAmount > 0)
                {
                    if (birthdayPackage.DiscountedPrice == null)
                    {
                        birthdayPackage.DiscountedPrice = birthdayPackage.Price - discountAmount;
                        birthdayPackage.HasDiscountsApplied = true;
                    }

                    else if (birthdayPackage.DiscountedPrice != null)
                    {
                        birthdayPackage.DiscountedPrice = birthdayPackage.DiscountedPrice - discountAmount;
                    }   
                }
                _context.Entry(birthdayPackage).State = EntityState.Modified;        
            }
            await _context.SaveChangesAsync();
            }
        }

        public async Task ResetBirthdayPackageDiscountedPrice(BirthdayPackage birthdayPackage)
        {     
            var birthdayPackageDiscounts = await _context.BirthdayPackageDiscounts
                .Where(x => x.BirthdayPackageId == birthdayPackage.Id).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackageDiscounts.Select(x => x.BirthdayPackageId);

            var list = await _context.BirthdayPackages.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var package in list)
                {
                    var discountPercentage2 = await _context.BirthdayPackageDiscounts
                        .Where(x => x.BirthdayPackageId == birthdayPackage.Id 
                        && x.BirthdayPackageId == package.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * birthdayPackage.Price;
                
                    if  (discountAmount > 0)
                        {          
                            birthdayPackage.DiscountedPrice = birthdayPackage.DiscountedPrice + discountAmount;

                            if (birthdayPackage.DiscountedPrice == birthdayPackage.Price)
                            {
                                birthdayPackage.DiscountedPrice = null;
                                birthdayPackage.HasDiscountsApplied = null;
                            }
                        }

                    _context.Entry(birthdayPackage).State = EntityState.Modified;        
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task ResetBirthdayPackageDiscountedPriceDueToDiscountExpiry
            (IEnumerable<BirthdayPackage> birthdayPackages)
        {
            IEnumerable<int> ids = birthdayPackages.Select(x => x.Id);

            var birthdayPackageDiscounts = await _context.BirthdayPackageDiscounts
                .Include(X => X.Discount).Include(x => x.BirthdayPackage)
                .Where(x => ids.Contains(x.BirthdayPackageId)).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackageDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddDays(-1))
                {
                    await ResetBirthayPackageDiscountedPrice(discount);
                }
            }             
        }

        public async Task ResetBirthayPackageDiscountedPrice(Discount discount)
        {     
            var birthdayPackageDiscounts = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackageDiscounts.Select(x => x.BirthdayPackageId);

            var list = await _context.BirthdayPackages.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    var discountPercentage2 = await _context.BirthdayPackageDiscounts
                        .Where(x => x.DiscountId == discount.Id 
                        && x.BirthdayPackageId == item.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                    if  (discountAmount > 0)
                        {          
                            item.DiscountedPrice = item.DiscountedPrice + discountAmount;

                            if (item.DiscountedPrice == item.Price)
                            {
                                item.DiscountedPrice = null;
                                item.HasDiscountsApplied = null;
                            }
                        }

                    _context.Entry(item).State = EntityState.Modified;        
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBirthdayPackageWithDiscount(Discount discount)
        {     
            var birthdayPackafeDiscounts = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackafeDiscounts.Select(x => x.BirthdayPackageId);

            var list = await _context.BirthdayPackages.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

            foreach (var item in list)
            {
                var discountPercentage2 = await _context.BirthdayPackageDiscounts
                    .Where(x => x.DiscountId == discount.Id && x.BirthdayPackageId == item.Id).FirstOrDefaultAsync();

                decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                if (discountAmount > 0)
                {
                    if (item.DiscountedPrice == null)
                    {
                        item.DiscountedPrice = item.Price - discountAmount;
                        item.HasDiscountsApplied = true;
                    }

                    else if (item.DiscountedPrice != null)
                    {
                        item.DiscountedPrice = item.DiscountedPrice - discountAmount;
                    }   
                }
                _context.Entry(item).State = EntityState.Modified;        
            }
            await _context.SaveChangesAsync();
            }
        }
   
      public async Task UpdateBirthdayPackageWithDiscount1(BirthdayPackage birthdayPackage)
        {     
            var birthdayPackakgeDiscounts = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.BirthdayPackageId == birthdayPackage.Id).ToListAsync();
            
            foreach (var item in birthdayPackakgeDiscounts)
            {
                var discount = await _context.Discounts.FirstOrDefaultAsync(x => x.Id == item.DiscountId);
                decimal discountPercentage = discount.DiscountPercentage;

                  var discountAmount = (discountPercentage / 100) * birthdayPackage.Price;
                
                if (discountAmount > 0)
                {
                    if (birthdayPackage.DiscountedPrice == null)
                    {
                        birthdayPackage.DiscountedPrice = birthdayPackage.Price - discountAmount;
                        birthdayPackage.HasDiscountsApplied = true;
                    }

                    else if (birthdayPackage.DiscountedPrice != null)
                    {
                        birthdayPackage.DiscountedPrice = birthdayPackage.DiscountedPrice - discountAmount;
                    }   
                    _context.Entry(birthdayPackage).State = EntityState.Modified;        
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<decimal> DiscountSum(BirthdayPackage birthdayPackage)
        {
            decimal discountsum = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.BirthdayPackageId == birthdayPackage.Id).SumAsync(x => x.Discount.DiscountPercentage);
        
            return discountsum;
        }
        
        public async Task DiscountSum1(IEnumerable<BirthdayPackage> birthdayPackages,
            IEnumerable<BirthdayPackageDto> birthdayPackagesDto)
        {
            IEnumerable<int> ids = birthdayPackages.Select(x => x.Id);

            var list = birthdayPackagesDto.Where(x => ids.Contains(x.Id)).ToList();

            foreach (var item in list)
            {
                item.DiscountSum = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                        .Where(x => x.BirthdayPackageId == item.Id)
                        .SumAsync(x => x.Discount.DiscountPercentage);
            }
        }

        // locations
        public async Task<Location1> FindLocationById(int id)
        {
            return await _context.Locations.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Location1>> GetAllLocations(QueryParameters queryParameters)
        {
            IQueryable<Location1> locations = _context.Locations
                .Include(x => x.Country).AsQueryable().OrderBy(x => x.City);

            if (queryParameters.HasQuery())
            {
                locations = locations.Where(t => t.City.Contains(queryParameters.Query));
            }

            locations = locations.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await locations.ToListAsync();
        }

        public async Task<int> GetCountForLocations()
        {
            return await _context.Locations.CountAsync();
        }

        public async Task CreateLocation(Location1 location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLocation(Location1 location)
        {
            _context.Entry(location).State = EntityState.Modified;        
            await _context.SaveChangesAsync();
        }

        // messages
        public async Task<ApplicationUser> GetAdmin()
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == "admin");
        }

        public async Task CreateMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        // servicesincluded
        public async Task AddServiceIncluded(ServiceIncluded serviceIncluded)
        {
            _context.ServicesIncluded.Add(serviceIncluded);

            await _context.SaveChangesAsync();
        }

        public async Task<ServiceIncluded> GetServiceIncludedById(int id)
        {
            return await _context.ServicesIncluded.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateServiceIncluded(ServiceIncluded serviceIncluded)
        {    
            _context.Entry(serviceIncluded).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        public async Task<List<ServiceIncluded>> GetAllServicesIncluded(QueryParameters queryParameters)
        {
            IQueryable<ServiceIncluded> servicesIncluded = _context.ServicesIncluded
                .AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                servicesIncluded = servicesIncluded.Where(t => t.Name.Contains(queryParameters.Query));
            }

            servicesIncluded = servicesIncluded.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await servicesIncluded.ToListAsync();
        }

        public async Task<int> GetCountForServicesIncluded()
        {
            return await _context.ServicesIncluded.CountAsync();
        }


        // blogs
        public async Task AddBlog(Blog blog)
        {
            _context.Blogs.Add(blog);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBlog(Blog blog)
        {    
            _context.Entry(blog).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        public async Task<List<Blog>> GetAllBlogsForUser(int userId, QueryParameters queryParameters)
        {
            IQueryable<Blog> blogs = _context.Blogs.Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUserId == userId)
                .AsQueryable().OrderBy(x => x.Title);

            if (queryParameters.HasQuery())
            {
                blogs = blogs.Where(t => t.Title.Contains(queryParameters.Query));
            }

            blogs = blogs.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await blogs.ToListAsync();
        }

        public async Task<int> GetCountForBlogsForUser(int userId)
        {
            return await _context.Blogs.Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUserId == userId).CountAsync();
        }

         public async Task<List<Blog>> GetAllBlogs(QueryParameters queryParameters)
        {
            IQueryable<Blog> blogs = _context.Blogs.AsQueryable().OrderBy(x => x.Title);

            if (queryParameters.HasQuery())
            {
                blogs = blogs.Where(t => t.Title.Contains(queryParameters.Query));
            }

            blogs = blogs.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await blogs.ToListAsync();
        }

        public async Task<int> GetCountForBlogs()
        {
            return await _context.Blogs.CountAsync();
        }

        public async Task<Blog> GetBlogById(int id)
        {
            return await _context.Blogs.Include(x => x.ApplicationUser).FirstOrDefaultAsync(p => p.Id == id);
        }

        // blogcomments
        public async Task AddBlogComment(BlogComment blogComment)
        {
            _context.BlogComments.Add(blogComment);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBlogComment(BlogComment blogComment)
        {    
            _context.Entry(blogComment).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        public async Task<BlogComment> GetBlogCommentById(int id)
        {
            return await _context.BlogComments.Include(x => x.ApplicationUser).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<BlogComment>> GetAllBlogComments(int blogId)
        {
            return await _context.BlogComments.Include(x => x.ApplicationUser)
                .Where(x => x.BlogId == blogId).ToListAsync();
        }

        public void DeleteBlogComment(BlogComment blogComment)
        {
            _context.BlogComments.Remove(blogComment);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }


    }
}








