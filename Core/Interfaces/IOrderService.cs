using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<CustomerOrder> CreateOrder(string buyerEmail, int deliveryMethod, 
            string basketId, ShippingAddress shippingAddress);
        Task<CustomerOrder> CreateOrder2(string buyerEmail, int shippingOptionId, 
            int paymentOption, string basketId, ShippingAddress shippingAddress);
        Task<CustomerOrder> CreateOrder3(string buyerEmail, int shippingOptionId, 
            int paymentOptionId, string basketId, ShippingAddress shippingAddress);
        Task<CustomerOrder> GetOrderById(int id, string customerEmail);
        Task<List<CustomerOrder>> GetOrdersForCustomer(string customerEmail);
        Task<bool> CheckIfBasketItemQuantityExceedsItemStackQuantity(string basketId);

        Task<List<ShippingOption>> GetShippingOptions();
        Task<List<PaymentOption>> GetPayingOptions();
        Task<List<Country>> GetCountries();
        Task<Country> GetCountryById(int id);
        Task<PaymentOption> GetStripePaymentOption();
        string GetCountryName(int id);
        decimal GetShippingOptionPrice(int id);
        public string GetPaymentOptionName(int id);



    }
}