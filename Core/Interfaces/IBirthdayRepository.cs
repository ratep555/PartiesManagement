using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IBirthdayRepository
    {
        Task AddBirthday(Birthday birthday);
        Task UpdateBirthday(Birthday birthday);
        Task<List<BirthdayPackage>> GetAllBirthdayPackages(QueryParameters queryParameters);
        Task<int> GetCountForBirthdayPackages();
    }
}