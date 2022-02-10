using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PartiesContext _context;
        public OrderRepository(PartiesContext context)
        {
            _context = context;
        }

        public Task<CustomerOrder> CreateOrder(string buyerEmail, int deliveryMethod, 
            string basketId, ShippingAddress shippingAddress)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Country>> GetCountries()
        {
            return await _context.Countries.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Country> GetCountryById(int id)
        {
            return await _context.Countries.FirstOrDefaultAsync(p => p.Id == id);        
        }

        public async Task<CustomerOrder> GetOrderById(int id, string customerEmail)
        {
            return await _context.CustomerOrders.Include(x => x.ShippingOption).Include(x => x.ShippingAddress)
                .Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id && x.CustomerEmail == customerEmail);
        }

        public async Task<List<CustomerOrder>> GetOrdersForCustomer(string customerEmail)
        {
            return await _context.CustomerOrders.Include(x => x.ShippingOption).Include(x => x.ShippingAddress)
                .Include(x => x.OrderItems).Where(x => x.CustomerEmail == customerEmail).ToListAsync();
        }

        public async Task<List<ShippingOption>> GetShippingOptions()
        {
            return await _context.ShippingOptions.OrderByDescending(x => x.Price).ToListAsync();
        }

        void IOrderRepository.CreateCustomerOrder(CustomerOrder customerOrder)
        {
            _context.CustomerOrders.Add(customerOrder);
        }

        public string GetCountryName(int id)
        {
            return _context.Countries.Where(x => x.Id == id).First().Name;
        }

        public async Task<PaymentOption> GetStripePaymentOption()
        {
            return await _context.PaymentOptions.FirstOrDefaultAsync(x => x.Name == "Stripe");
        }

        public async Task<ShippingOption> FindShippingOptionById(int id)
        {
            return await _context.ShippingOptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CustomerOrder> FindOrderByPaymentIntentId(string paymentIntentIid)
        {
            return await _context.CustomerOrders.FirstOrDefaultAsync(x => x.PaymentIntentId == paymentIntentIid);
        }

        public void DeleteCustomerOrder(CustomerOrder order)
        {
            _context.CustomerOrders.Remove(order);
        }

        public void UpdateCustomerOrder(CustomerOrder order)
        {
            _context.CustomerOrders.Update(order);
        }

        public string GetPaymentOptionName(int id)
        {
            return _context.PaymentOptions.Where(x => x.Id == id).First().Name;
        }

        public decimal GetShippingOptionPrice(int id)
        {
            return _context.ShippingOptions.Where(x => x.Id == id).First().Price;
        }

        public async Task<List<PaymentOption>> GetPayingOptions()
        {
            return await _context.PaymentOptions.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task FillingItemWarehousesQuantity(int id, int basketItemQuantity)
        {
            var list = await _context.ItemWarehouses.Where(x => x.ItemId == id).ToListAsync();

            int stockquantity = 0;

            foreach (var item in list)
            {
                stockquantity = item.StockQuantity;
            }

            foreach (var item in list)
            {
                if (item.StockQuantity >= basketItemQuantity)
                {
                    item.StockQuantity = item.StockQuantity - basketItemQuantity;
                    await _context.SaveChangesAsync();
                    basketItemQuantity = 0;
                }

                else if(item.StockQuantity < basketItemQuantity)
                {
                    var sum1 = basketItemQuantity - item.StockQuantity;

                    item.StockQuantity = 0;
                    await _context.SaveChangesAsync();

                    var model = await _context.ItemWarehouses
                        .FirstOrDefaultAsync(x => x.ItemId == item.ItemId && x.StockQuantity > 0);     
                    
                    model.StockQuantity = model.StockQuantity - sum1;
                    await _context.SaveChangesAsync();          
                }
                basketItemQuantity = 0;


                  /* sum += (basketItemQuantity - item.StockQuantity);
                var model = await _context.ItemWarehouses
                    .FirstOrDefaultAsync(x => x.ItemId == item.ItemId && x.StockQuantity > 0);
                
                model.StockQuantity = stockquantity - sum;
                await _context.SaveChangesAsync();  */
            }




        }
    }
}







