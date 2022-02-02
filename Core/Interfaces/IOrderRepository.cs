using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        void CreateCustomerOrder(CustomerOrder customerOrder);
        Task<CustomerOrder> GetOrderById(int id, string buyerEmail);
        Task<List<CustomerOrder>> GetOrdersForCustomer(string customerEmail);

        Task<Country> GetCountryById(int id);
        Task<CustomerOrder> CreateOrder(string buyerEmail, int deliveryMethod, 
            string basketId, ShippingAddress shippingAddress);
        Task<List<ShippingOption>> GetShippingOptions();
        Task<List<PaymentOption>> GetPayingOptions();
        Task<List<Country>> GetCountries();
        public string GetCountryName(int id);
        Task<PaymentOption> GetStripePaymentOption();
        
        // možda ne trebaš public tu!
        public decimal GetShippingOptionPrice(int id);
        public string GetPaymentOptionName(int id);
        Task<ShippingOption> FindShippingOptionById(int id);
        Task<CustomerOrder> FindOrderByPaymentIntentId(string paymentIntentId);
        void DeleteCustomerOrder(CustomerOrder order);
        void UpdateCustomerOrder(CustomerOrder order);



    }
}




