using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly PartiesContext _context;
        public ItemRepository(PartiesContext context)
        {
            _context = context;
        }

        public void AddItem(Item item)
        {
            _context.Items.Add(item);
        }
        public void AddItemWithDiscount(Item item)
        {
            _context.Items.Add(item);
        }

        public async Task<List<Item>> GetAllItems(QueryParameters queryParameters)
        {
            IQueryable<Item> items = _context.Items.Include(x => x.ItemDiscounts)
                .ThenInclude(x => x.Discount).AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                items = items.Where(t => t.Name.Contains(queryParameters.Query));
            }

            items = items.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await items.ToListAsync();
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Discount>> GetAllDiscounts()
        {
            return await _context.Discounts.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Manufacturer>> GetAllManufacturers()
        {
            return await _context.Manufacturers.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<int> GetCountForItems()
        {
            return await _context.Items.CountAsync();
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.Include(x => x.ItemCategories).ThenInclude(x => x.Category)
                .Include(x => x.ItemDiscounts).ThenInclude(x => x.Discount)
                .Include(x => x.ItemManufacturers).ThenInclude(x => x.Manufacturer)
                .Include(x => x.ItemTags).ThenInclude(x => x.Tag).
            FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateItem(ItemEditDto itemdto)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == itemdto.Id);
            
            _context.Entry(item).State = EntityState.Modified;        
        }

        public async Task DeleteExistingEntities(Item itemdb, ItemEditDto itemdto)
        {
            foreach (var id in itemdto.CategoriesIds)
            {
                var itemcategory= new ItemCategory() { CategoryId = id };
                await _context.SaveChangesAsync();
            }

            foreach (var id in itemdto.ManufacturersIds)
            {
                var itemmanufcturer = new ItemManufacturer() { ManufacturerId = id };
                                await _context.SaveChangesAsync();

            }


            foreach (var id in itemdto.TagsIds)
            {
                var itemtag= new ItemTag() { TagId = id };
                                await _context.SaveChangesAsync();

            }

            _context.Entry(itemdb).State = EntityState.Modified;        

            await _context.SaveChangesAsync();
           

        }

        public List<int> CategoryIds(int id)
        {
            IEnumerable<int> ids = _context.ItemCategories.Where(x => x.ItemId == id).Select(x => x.CategoryId);

            return ids.ToList();
        }

        public List<int> ManufacturerIds(int id)
        {
            IEnumerable<int> ids = _context.ItemManufacturers.Where(x => x.ItemId == id).Select(x => x.ManufacturerId);

            return ids.ToList();
        }

        public List<int> TagIds(int id)
        {
            IEnumerable<int> ids = _context.ItemTags.Where(x => x.ItemId == id).Select(x => x.TagId);

            return ids.ToList();
        }

        public async Task<List<Category>> GetNonSelectedCategories(List<int> ids)
        {
            return await _context.Categories.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Discount>> GetNonSelectedDiscounts(List<int> ids)
        {
            return await _context.Discounts.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Manufacturer>> GetNonSelectedManufacturers(List<int> ids)
        {
            return await _context.Manufacturers.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Tag>> GetNonSelectedTags(List<int> ids)
        {
            return await _context.Tags.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }
        public async Task<List<Category>> GetSelectedCategories(List<int> ids)
        {
            return await _context.Categories.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
        public async Task<List<Manufacturer>> GetSelectedManufacturers(List<int> ids)
        {
            return await _context.Manufacturers.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
        public async Task<List<Tag>> GetSelectedTags(List<int> ids)
        {
            return await _context.Tags.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task AddItem1(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItem1(Item item)
        {
            _context.Entry(item).State = EntityState.Modified; 
            await _context.SaveChangesAsync();       
        }

        // rating

        public async Task<Rating> FindCurrentRate(int itemId, int userId)
        {
                return await _context.Ratings.Include(x => x.Customer)
                .Where(x => x.ItemId == itemId && x.ApplicationUserId == userId)
                .FirstOrDefaultAsync();        
        }

        public async Task AddRating(RatingDto ratingDto, int userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var rating = new Rating();
            rating.ItemId = ratingDto.ItemId;
            rating.Rate = ratingDto.Rating;
            rating.ApplicationUserId = user.Id;

            _context.Ratings.Add(rating);
        }

        public async Task<bool> CheckIfCustomerHasOrderedThisItem(int itemId, string email)
        {
            var orderitem = await _context.OrderItems
                .FirstOrDefaultAsync(x => x.BasketItemOrdered.BasketItemOrderedId == itemId);
            
            var orders = await _context.CustomerOrders.Include(x => x.OrderItems)
                .Where(x => x.CustomerEmail == email &&
                 x. OrderItems.Contains(orderitem)).ToListAsync();

           if (!orders.Any())
           {
               return true;
           }
           return false;
        }

        public async Task<double> AverageVote(int id)
        {
            var average = await _context.Ratings.Where(x => x.ItemId == id).AverageAsync(x => x.Rate);

            return average;
        }

        public async Task<bool> ChechIfAny(int id)
        {
           return await _context.Ratings.AnyAsync(x => x.ItemId == id);          
        }


        // discounts
        public async Task<Discount> FindDiscountById(int id)
        {
            return await _context.Discounts.Include(x => x.ItemDiscounts).ThenInclude(x => x.Item)
                .Include(X => X.CategoryDiscounts).ThenInclude(X => X.Category)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Item>> GetNonSelectedItems(List<int> ids)
        {
            return await _context.Items.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }

        public async Task AddDiscount(Discount discount)
        {
            discount.StartDate = discount.StartDate.ToLocalTime();
            discount.EndDate = discount.EndDate.ToLocalTime();
       
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscount(Discount discount)
        {    
            discount.StartDate = discount.StartDate.ToLocalTime();
            discount.EndDate = discount.EndDate.ToLocalTime();

            _context.Entry(discount).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }
 
        public async Task UpdateItemWithDiscount(Discount discount)
        {     
            var itemDiscounts = await _context.ItemDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = itemDiscounts.Select(x => x.ItemId);

            var list = await _context.Items.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

            foreach (var item in list)
            {
                var discountPercentage2 = await _context.ItemDiscounts
                    .Where(x => x.DiscountId == discount.Id && x.ItemId == item.Id).FirstOrDefaultAsync();

                decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                if (discountAmount > 0)
                {
                    if (item.DiscountedPrice == null)
                    {
                        item.DiscountedPrice = item.Price - discountAmount;
                        discountPercentage2.IsAppliedOnItem = true;
                        item.HasDiscountsApplied = true;
                    }

                    else if (item.DiscountedPrice != null)
                    {
                        item.DiscountedPrice = item.DiscountedPrice - discountAmount;
                        discountPercentage2.IsAppliedOnItem = true;
                    }   
                }
                _context.Entry(item).State = EntityState.Modified;        
            }
            await _context.SaveChangesAsync();
            }
        }
        public async Task ResetItemDiscountedPrice(Discount discount)
        {     
            var itemDiscounts = await _context.ItemDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = itemDiscounts.Select(x => x.ItemId);

            var list = await _context.Items.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    var discountPercentage2 = await _context.ItemDiscounts
                        .Where(x => x.DiscountId == discount.Id && x.ItemId == item.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                    if  (discountAmount > 0)
                        {          
                            item.DiscountedPrice = item.DiscountedPrice + discountAmount;
                            discountPercentage2.IsAppliedOnItem = false;

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

        public async Task<decimal> DiscountSum(Item item)
        {
            decimal discountsum = await _context.ItemDiscounts.Include(X => X.Discount)
                .Where(x => x.ItemId == item.Id).SumAsync(x => x.Discount.DiscountPercentage);
            
            return discountsum;
        }

        public async Task DeleteDiscount(Discount discount)
        {
            _context.Discounts.Remove(discount);

            await ResetItemDiscountedPrice(discount);

            await _context.SaveChangesAsync();
        }

        public async Task ResetItemDiscountedPriceDueToDiscountExpiry(IEnumerable<Item> items)
        {
            IEnumerable<int> ids = items.Select(x => x.Id);

            var itemDiscounts = await _context.ItemDiscounts.Include(X => X.Discount).Include(x => x.Item)
                .Where(x => ids.Contains(x.ItemId)).ToListAsync();

            IEnumerable<int> ids1 = itemDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddDays(-1))
                {
                    await ResetItemDiscountedPrice(discount);
                }
            }             
        }
        public async Task<List<Discount>> GetAllDiscounts(QueryParameters queryParameters)
        {
            IQueryable<Discount> discounts = _context.Discounts.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                discounts = discounts.Where(t => t.Name.Contains(queryParameters.Query));
            }

            discounts = discounts.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await discounts.ToListAsync();
        }

        public async Task<int> GetCountForDiscounts()
        {
            return await _context.Discounts.CountAsync();
        }

        public async Task<List<Item>> GetAllItemsForDiscounts()
        {
            return await _context.Items.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task UpdateItemWithCategoryDiscount(Discount discount)
        {    
            var categoryDiscounts = await _context.CategoryDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids3 = categoryDiscounts.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids3.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = categories.Select(x => x.Id);

            var itemcategories = await _context.ItemCategories.Where(x => ids4.Contains(x.CategoryId)).ToListAsync();
            
            IEnumerable<int> ids5 = itemcategories.Select(x => x.ItemId);

            var list = await _context.Items
                .Where(x => ids5.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

            foreach (var item in list)
            {
                var categoryPercentage2 = await _context.CategoryDiscounts.Include(x => x.Discount)
                    .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                decimal discountPercentage1 = categoryPercentage2.Discount.DiscountPercentage;

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

        public async Task ResetCategoryDiscountedPrice(Discount discount)
        {   
            var categoryDiscounts = await _context.CategoryDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids3 = categoryDiscounts.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids3.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = categories.Select(x => x.Id);

            var itemcategories = await _context.ItemCategories.Where(x => ids4.Contains(x.CategoryId)).ToListAsync();
            
            IEnumerable<int> ids5 = itemcategories.Select(x => x.ItemId);

            var list = await _context.Items
                .Where(x => ids5.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    var categoryPercentage2 = await _context.CategoryDiscounts.Include(x => x.Discount)
                    .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = categoryPercentage2.Discount.DiscountPercentage;

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

       

        public async Task ResetCategoryDiscountedPriceDueToDiscountExpiry(IEnumerable<Item> items)
        {
            IEnumerable<int> ids = items.Select(x => x.Id);

            var itemDiscounts = await _context.ItemDiscounts.Include(X => X.Discount).Include(x => x.Item)
                .Where(x => ids.Contains(x.ItemId)).ToListAsync();

            IEnumerable<int> ids1 = itemDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddDays(-1))
                {
                    await ResetItemDiscountedPrice(discount);
                }
            }             
        }
      
    
    }
}








