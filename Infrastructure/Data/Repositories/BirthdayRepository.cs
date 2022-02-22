using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
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
        public async Task AddBirthday(Birthday birthday)
        {
            var birthdayPackage = await _context.BirthdayPackages
                .FirstOrDefaultAsync(x => x.Id == birthday.BirthdayPackageId);
            
            int minutes = birthdayPackage.Duration;

            birthday.EndDateAndTime = birthday.StartDateAndTime.AddMinutes(minutes);
            birthday.Price = birthdayPackage.Price;

            _context.Birthdays.Add(birthday);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBirthday(Birthday birthday)
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

    }
}








