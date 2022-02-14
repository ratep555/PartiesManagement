using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllItems(QueryParameters queryParameters);
        Task<int> GetCountForItems();
        Task<List<Category>> GetAllCategories();
        Task<List<Discount>> GetAllDiscounts();
        Task<List<Manufacturer>> GetAllManufacturers();
        Task<List<Tag>> GetAllTags();
        Task<Item> GetItemById(int id);
        void AddItem(Item item);
        Task AddItem1(Item item);
        Task UpdateItem(ItemEditDto item);
        Task UpdateItem1(Item item);
        Task<List<Category>> GetNonSelectedCategories(List<int> ids);
        Task<List<Discount>> GetNonSelectedDiscounts(List<int> ids);
        Task<List<Category>> GetSelectedCategories(List<int> ids);
        Task<List<Manufacturer>> GetSelectedManufacturers(List<int> ids);
        Task<List<Tag>> GetSelectedTags(List<int> ids);
        Task DeleteExistingEntities(Item itemdb, ItemEditDto itemdto);

        Task<List<Manufacturer>> GetNonSelectedManufacturers(List<int> ids);
        Task<List<Tag>> GetNonSelectedTags(List<int> ids);
        List<int> CategoryIds(int id);
        List<int> ManufacturerIds(int id);
        List<int> TagIds(int id);

        // rating
        Task<Rating> FindCurrentRate(int itemId, int userId);
        Task AddRating(RatingDto ratingDto, int userId);
        Task<bool> CheckIfCustomerHasOrderedThisItem(int itemId, string email);
        Task<double> AverageVote(int id);
        Task<bool> ChechIfAny(int id);

        // discounts
        Task<Discount> FindDiscountById(int id);
        Task<List<Item>> GetNonSelectedItems(List<int> ids);
        Task AddDiscount(Discount discount);
        Task UpdateDiscount(Discount discount);
        Task UpdateItemWithDiscount(Discount discount);
        Task ResetItemDiscountedPrice(Discount discount);
        Task<decimal> DiscountSum(Item item);
        Task DeleteDiscount(Discount discount);
        Task ResetItemDiscountedPriceDueToDiscountExpiry(IEnumerable<Item> items);
        Task<List<Discount>> GetAllDiscounts(QueryParameters queryParameters);
        Task<int> GetCountForDiscounts();
        Task<List<Item>> GetAllItemsForDiscounts();
        Task UpdateItemWithCategoryDiscount(Discount discount);
        Task ResetCategoryDiscountedPrice(Discount discount);


    }
}





