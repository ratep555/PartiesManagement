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

        public async Task AddItem7(Item item)
        {
            _context.Items.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemWithDiscount7(Item item)
        {     
            var itemDiscounts = await _context.ItemDiscounts.Include(X => X.Discount)
                .Where(x => x.ItemId == item.Id).ToListAsync();

            IEnumerable<int> ids1 = itemDiscounts.Select(x => x.ItemId);

            var list = await _context.Items.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

            foreach (var products in list)
            {
                var discountPercentage2 = await _context.ItemDiscounts
                    .Where(x => x.ItemId == item.Id && x.ItemId == products.Id).FirstOrDefaultAsync();

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
     
        public void AddItemWithDiscount(Item item)
        {
            _context.Items.Add(item);
        }

        public async Task<List<Item>> GetAllItems(QueryParameters queryParameters)
        {
            var itemManufacturers = await _context.ItemManufacturers
                .Where(x => x.ManufacturerId == queryParameters.ManufacturerId).ToListAsync();
            
            IEnumerable<int> ids = itemManufacturers.Select(x => x.ItemId);

            var itemTags = await _context.ItemTags
                .Where(x => x.TagId == queryParameters.TagId).ToListAsync();
            
            IEnumerable<int> ids1 = itemTags.Select(x => x.ItemId);

            var itemCategories = await _context.ItemCategories
                .Where(x => x.CategoryId == queryParameters.CategoryId).ToListAsync();
            
            IEnumerable<int> ids2 = itemCategories.Select(x => x.ItemId);

            IQueryable<Item> items = _context.Items.Include(x => x.ItemDiscounts)
                .ThenInclude(x => x.Discount).Include(x => x.ItemCategories).ThenInclude(x => x.Category)
                .Include(x => x.ItemTags).ThenInclude(x => x.Tag)
                .Include(x => x.ItemManufacturers).ThenInclude(x => x.Manufacturer)
                .AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                items = items.Where(t => t.Name.Contains(queryParameters.Query));
            }

            if (queryParameters.ManufacturerId.HasValue)
            {
                items = items.Where(x => ids.Contains(x.Id));
            }

            if (queryParameters.TagId.HasValue)
            {
                items = items.Where(x => ids1.Contains(x.Id));

            }
            if (queryParameters.CategoryId.HasValue)
            {
                items = items.Where(x => ids2.Contains(x.Id));
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

        public async Task UpdateItem7(Item item)
        {    
            _context.Entry(item).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        public async Task ResetItemDiscountedPrice7(Item item)
        {     
            var itemDiscounts = await _context.ItemDiscounts
                .Where(x => x.ItemId == item.Id).ToListAsync();

            IEnumerable<int> ids1 = itemDiscounts.Select(x => x.ItemId);

            var list = await _context.Items.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var product in list)
                {
                    var discountPercentage2 = await _context.ItemDiscounts
                        .Where(x => x.ItemId == item.Id && x.ItemId == product.Id).FirstOrDefaultAsync();

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
        public async Task<List<Manufacturer1>> GetNonSelectedManufacturers1(List<int> ids)
        {
            return await _context.Manufacturers1.Where(x => !ids.Contains(x.Id)).ToListAsync();
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
                .Include(x => x.ManufacturerDiscounts).ThenInclude(x => x.Manufacturer1)
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

            var itemCategory = await _context.ItemCategories.Where(x => x.ItemId == item.Id).ToListAsync();
            IEnumerable<int> ids1 = itemCategory.Select(x => x.CategoryId);

            decimal discountsum1 = await _context.CategoryDiscounts.Include(X => X.Discount)
                .Where(x => ids1.Contains(x.CategoryId)).SumAsync(x => x.Discount.DiscountPercentage);

            var manufacturers = await _context.Manufacturers1.Where(x => x.Id == item.Manufacturer1Id).ToListAsync();
            IEnumerable<int> ids = manufacturers.Select(x => x.Id);

            decimal discountsum2 = await _context.ManufacturerDiscounts.Include(X => X.Discount)
                .Where(x => ids.Contains(x.Manufacturer1Id)).SumAsync(x => x.Discount.DiscountPercentage);
            
            var result = discountsum + discountsum1 + discountsum2;
            
            return result;
        }

        public async Task DeleteDiscount(Discount discount)
        {
            await ResetItemDiscountedPrice(discount);
            await ResetCategoryDiscountedPrice(discount);
            await ResetManufacturerDiscountedPrice(discount);

            _context.Discounts.Remove(discount);

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

        public async Task<List<Category>> GetAllCategoriesForDiscounts()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Manufacturer1>> GetAllManufacturersForDiscounts()
        {
            return await _context.Manufacturers1.OrderBy(x => x.Name).ToListAsync();
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

            var itemCategories = await _context.ItemCategories
                .Where(x => ids.Contains(x.ItemId)).ToListAsync();

            IEnumerable<int> ids1 = itemCategories.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids1.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = categories.Select(x => x.Id);

            var categoryDiscounts = await _context.CategoryDiscounts
                .Where(x => ids4.Contains(x.CategoryId)).ToListAsync();

            IEnumerable<int> ids5 = categoryDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids5.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddDays(-1))
                {
                    await ResetCategoryDiscountedPrice(discount);
                }
            }             
        }

        public async Task<List<Manufacturer1>> GetManufacturers()
        {
            return await _context.Manufacturers1.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task UpdateItemWithManufacturerDiscount(Discount discount)
        {    
            var manufacturerDiscounts = await _context.ManufacturerDiscounts
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids3 = manufacturerDiscounts.Select(x => x.Manufacturer1Id);

            var manufacturers = await _context.Manufacturers.Where(x => ids3.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = manufacturers.Select(x => x.Id);

            var list = await _context.Items.Include(x => x.Manufacturer1)
                .Where(x => ids4.Contains(x.Manufacturer1.Id)).ToListAsync();

            if (list.Any())
            {
            foreach (var item in list)
            {
                var manufacturerPercentage2 = await _context.ManufacturerDiscounts.Include(x => x.Discount)
                    .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                decimal discountPercentage1 = manufacturerPercentage2.Discount.DiscountPercentage;

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

        public async Task ResetManufacturerDiscountedPrice(Discount discount)
        {   
            var manufacturerDiscounts = await _context.ManufacturerDiscounts
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids3 = manufacturerDiscounts.Select(x => x.Manufacturer1Id);

            var manufacturers = await _context.Manufacturers.Where(x => ids3.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = manufacturers.Select(x => x.Id);

            var list = await _context.Items.Include(x => x.Manufacturer1)
                .Where(x => ids4.Contains(x.Manufacturer1.Id)).ToListAsync();

            if (list.Any())
            {
            foreach (var item in list)
            {
                var manufacturerPercentage2 = await _context.ManufacturerDiscounts.Include(x => x.Discount)
                    .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                decimal discountPercentage1 = manufacturerPercentage2.Discount.DiscountPercentage;

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

        public async Task ResetManufacturerDiscountedPriceDueToDiscountExpiry(IEnumerable<Item> items)
        {
            IEnumerable<int> ids = items.Where(x => x.Manufacturer1Id != null).Select(x =>(int)x.Manufacturer1Id);

            var manufacturers = await _context.Manufacturers1.Where(x => ids.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = manufacturers.Select(x => x.Id);

            var manufacturerDiscounts = await _context.ManufacturerDiscounts
                .Where(x => ids4.Contains(x.Manufacturer1Id)).ToListAsync();

            IEnumerable<int> ids5 = manufacturerDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids5.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddDays(-1))
                {
                    await ResetManufacturerDiscountedPrice(discount);
                }
            }   
        }

        public async Task<List<Manufacturer1>> GetManufacturersAttributedToItems()
        {
            var itemManufacturers = await _context.ItemManufacturers.ToListAsync();

            IEnumerable<int> ids = itemManufacturers.Select(x => x.ManufacturerId);

            var manufacturers = await _context.Manufacturers1.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return manufacturers;
        }

        public async Task<List<Tag>> GetTagsAttributedToItems()
        {
            var itemTags = await _context.ItemTags.ToListAsync();

            IEnumerable<int> ids = itemTags.Select(x => x.TagId);

            var tags = await _context.Tags.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return tags;
        }

        public async Task<List<Category>> GetCategoriesAttributedToItems()
        {
            var itemCategories = await _context.ItemCategories.ToListAsync();

            IEnumerable<int> ids = itemCategories.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name).ToListAsync();

            return categories;
        }

        // paymentstatus1
        public int PaymentStatusPaid()
        {
            return _context.PaymentStatuses1.FirstOrDefaultAsync(x => x.Name =="Paid").Id;
        }
        public string PaymentStatusPending()
        {
            return _context.PaymentStatuses1.Where(x => x.Name =="Pending").First().Name;
        }
        public int PaymentStatusFailed()
        {
            return _context.PaymentStatuses1.FirstOrDefaultAsync(x => x.Name =="Failed").Id;
        }
        public int PaymentStatusVoided()
        {
            return _context.PaymentStatuses1.FirstOrDefaultAsync(x => x.Name =="Voided").Id;
        }


        // likes
        public async Task AddLike(int userId, int itemId)
        {
            var like = new Like()
            {
                ApplicationUserId = userId,
                ItemId = itemId
            };

            _context.Likes.Add(like);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserHasAlreadyLikedThisProduct(int userId, int itemId)
        {
            var like = await _context.Likes.
                FirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.ItemId == itemId);
            
            if(like!= null) return true;

            return false;  
        }

        public async Task<int> GetCountForLikes(int itemId)
        {
            return await _context.Likes.Where(x => x.ItemId == itemId).CountAsync();
        }


        // orders
        public async Task<CustomerOrder> GetOrderByIdForEditing(int id)
        {
            return await _context.CustomerOrders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<OrderStatus1>> GetAllOrderStatuses()
        {
            return await _context.OrderStatus1.OrderBy(x => x.Name).ToListAsync();
        }


        // itemwarehouses
        public async Task DecreasingItemWarehousesQuantity(int id, int quantity)
        {
            var list = await _context.ItemWarehouses
                .Where(x => x.ItemId == id && x.StockQuantity > 0).ToListAsync();

            foreach (var item in list)
            {
                int? reservedquantity  = item.ReservedQuantity;
                    
                if (item.StockQuantity >= quantity)
                {
                    item.StockQuantity = item.StockQuantity - quantity;
                 //   item.ReservedQuantity = ++item.ReservedQuantity ?? 1;
                  //  await _context.SaveChangesAsync();
                    quantity = 0;
                   // number = 0;
                }

                else if(item.StockQuantity < quantity)
                {
                    var model = await _context.ItemWarehouses
                        .FirstOrDefaultAsync(x => x.ItemId == item.ItemId && x.StockQuantity > 0);  

                    model.StockQuantity = model.StockQuantity - quantity;
                //    model.ReservedQuantity = ++model.ReservedQuantity ?? 1;
                    await _context.SaveChangesAsync();          
                }
                quantity = 0;
            }

            var model1 = await _context.ItemWarehouses
                .FirstOrDefaultAsync(X => X.ItemId == id && X.StockQuantity > 0);

            model1.ReservedQuantity = ++model1.ReservedQuantity ?? 1;
            
            await _context.SaveChangesAsync();
        }

        public async Task DecreasingItemWarehousesQuantity1(int id, int quantity)
        {
            var list = await _context.ItemWarehouses
                .Where(x => x.ItemId == id && x.StockQuantity > 0).ToListAsync();

            foreach (var item in list)
            {
                int result = 0;

                if (quantity > 1)
                {
                    if (item.StockQuantity >= 0)
                    {
                        if (item.StockQuantity >= quantity)
                        {
                            item.StockQuantity = item.StockQuantity - quantity;
                            item.ReservedQuantity = item.ReservedQuantity + quantity ?? quantity;
                            await _context.SaveChangesAsync();
                        }
                        else if (item.StockQuantity < quantity)
                        {
                            item.ReservedQuantity = item.ReservedQuantity + item.StockQuantity ?? item.StockQuantity;
                            result = quantity - item.StockQuantity; 
                            item.StockQuantity = 0;
                            await _context.SaveChangesAsync();
                            
                            var model = await _context.ItemWarehouses
                                .FirstOrDefaultAsync(x => x.StockQuantity > 0);
                            
                            model.StockQuantity = model.StockQuantity - result;
                            model.ReservedQuantity = model.ReservedQuantity + result ?? result;
                            await _context.SaveChangesAsync();
                        }
                        quantity = 0;
                    }
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task IncreasingItemWarehousesQuantity(int id, int quantity)
        {
            var list = await _context.ItemWarehouses.Where(x => x.ItemId == id
                && x.ReservedQuantity != null && x.ReservedQuantity > 0).ToListAsync();

            foreach (var item in list)
            {
                    if (item.StockQuantity != 0)
                    {
                        item.StockQuantity = item.StockQuantity += quantity;
                        item.ReservedQuantity = item.ReservedQuantity - quantity;
                        await _context.SaveChangesAsync();
                        
                    }

                    else if (item.StockQuantity == 0)
                    {
                        item.StockQuantity = item.StockQuantity += quantity;
                        item.ReservedQuantity = item.ReservedQuantity - quantity;
                        await _context.SaveChangesAsync();          
                    }
                    quantity = 0;
            }
        }
        public async Task RemovingReservedQuantityFromItemWarehouses(int itemId, int quantity)
        {
            var list = await _context.ItemWarehouses.Where(x => x.ItemId == itemId
                && x.ReservedQuantity != null && x.ReservedQuantity > 0).ToListAsync();

            if (quantity > 1)
            {
                foreach (var item in list)
                {
                    item.StockQuantity = item.StockQuantity += (int)item.ReservedQuantity;
                    item.ReservedQuantity = 0;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task AddItemWarehouse(ItemWarehouse itemWarehouse)
        {
            _context.ItemWarehouses.Add(itemWarehouse);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemWarehouse(ItemWarehouse itemWarehouse)
        {
            _context.Entry(itemWarehouse).State = EntityState.Modified;        

            await _context.SaveChangesAsync();
        }

        public async Task<List<ItemWarehouse>> GetAllItemWarehouses(QueryParameters queryParameters)
        {
            IQueryable<ItemWarehouse> itemWarehouses = _context.ItemWarehouses.Include(x => x.Item)
                .Include(x => x.Warehouse).AsQueryable().OrderBy(x => x.Item.Name);

            if (queryParameters.HasQuery())
            {
                itemWarehouses = itemWarehouses.Where(t => t.Item.Name.Contains(queryParameters.Query));
            }

            itemWarehouses = itemWarehouses.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await itemWarehouses.ToListAsync();
        }

        public async Task<int> GetCountForItemWarehouses()
        {
            return await _context.ItemWarehouses.CountAsync();
        }

        public async Task<ItemWarehouse> FindItemWarehouseByItemIdAndWarehouseId(int itemId, int warehouseId)
        {
            return await _context.ItemWarehouses.Include(x => x.Item).Include(x => x.Warehouse)
                .FirstOrDefaultAsync(x => x.ItemId == itemId && x.WarehouseId == warehouseId);
        }

        public async Task<List<Item>> GetAllItemsForItemWarehouses()
        {
            return await _context.Items.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Warehouse>> GetAllWarehousesForItemWarehouse()
        {
            return await _context.Warehouses.Include(x => x.Country).OrderBy(x => x.WarehouseName).ToListAsync();
        }

        public async Task UpdatingItemStockQuantityBasedOnWarehousesQuantity(List<Item> items)
        {
            IEnumerable<int> ids = items.Select(x => x.Id);

            foreach (var item in items)
            {
                
                item.StockQuantity = await _context.ItemWarehouses
                    .Where(x => ids.Contains(x.ItemId) && x.ItemId == item.Id)
                    .SumAsync(x => x.StockQuantity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddingNewStockQuantityToItemAndRemovingOldOne(Item item)
        {
            item.StockQuantity = await _context.ItemWarehouses.Where(x => x.ItemId == item.Id)
                .SumAsync(x => x.StockQuantity);

            await _context.SaveChangesAsync();
        }

      
    }
}















