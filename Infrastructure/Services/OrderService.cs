using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }

        public async Task<CustomerOrder> CreateOrder(string buyerEmail, int shippingOptionId, 
            string basketId, ShippingAddress shippingAddress)
        {
            var basket = await _basketRepository.GetBasket(basketId);

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ItemRepository.GetItemById(item.Id);

                var basketItemOrdered = new BasketItemOrdered(productItem.Id, productItem.Name);

                var orderItem = new OrderItem(basketItemOrdered, productItem.Price, item.Quantity, productItem.Picture);
                orderItems.Add(orderItem);
            }

            var shippingOption = await _unitOfWork.ShippingOptionRepository.GetShippingOptionById(shippingOptionId);
 
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            var existingOrder = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(basket.PaymentIntentId);

            if (existingOrder != null)
            {
                _unitOfWork.OrderRepository.DeleteCustomerOrder(existingOrder);

                await _paymentService.CreatingOrUpdatingPaymentIntent(basket.PaymentIntentId);
            }

            var customerOrder = new CustomerOrder(orderItems, buyerEmail, shippingAddress, shippingOption,
            subtotal, basket.PaymentIntentId);
            _unitOfWork.OrderRepository.CreateCustomerOrder(customerOrder);

            if (await _unitOfWork.SaveAsync()) return customerOrder;

            return null;
        }

        public async Task<CustomerOrder> CreateOrder2(string buyerEmail, int shippingOptionId, 
            int paymentOptionId, string basketId, ShippingAddress shippingAddress)
        {
            var basket = await _basketRepository.GetBasket(basketId);

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ItemRepository.GetItemById(item.Id);

                var basketItemOrdered = new BasketItemOrdered(productItem.Id, productItem.Name);

                var orderItem = new OrderItem(basketItemOrdered, productItem.Price, item.Quantity, productItem.Picture);
               
                if (productItem.DiscountedPrice != null)
                {
                    orderItem.Price = (decimal)productItem.DiscountedPrice;
                }

                orderItems.Add(orderItem);

               // productItem.StockQuantity = productItem.StockQuantity - item.Quantity;

                await _unitOfWork.OrderRepository.FillingItemWarehousesQuantity(productItem.Id, item.Quantity);
            }

            var shippingOption = await _unitOfWork.ShippingOptionRepository.GetShippingOptionById(shippingOptionId);

            var paymentOption = await _unitOfWork.ShippingOptionRepository.GetPaymentOptionById(paymentOptionId);
 
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

          //  var existingOrder = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(basket.PaymentIntentId);

            var customerOrder = new CustomerOrder(orderItems, buyerEmail, shippingAddress, shippingOption,
            paymentOption, subtotal, basket.PaymentIntentId);

            _unitOfWork.OrderRepository.CreateCustomerOrder(customerOrder);

            if (await _unitOfWork.SaveAsync()) return customerOrder;

            return null;        
        }

        public async Task<bool> CheckIfBasketItemQuantityExceedsItemStackQuantity(string basketId)
        {
            var basket = await _basketRepository.GetBasket(basketId);

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ItemRepository.GetItemById(item.Id);

                if (productItem.StockQuantity < 0) return true;              
            }
            return false;
        }

        public async Task<CustomerOrder> CreateOrder3(string buyerEmail, int shippingOptionId, 
            int paymentOptionId, string basketId, ShippingAddress shippingAddress)
        {
            var basket = await _basketRepository.GetBasket(basketId);

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ItemRepository.GetItemById(item.Id);

                var basketItemOrdered = new BasketItemOrdered(productItem.Id, productItem.Name);

                var orderItem = new OrderItem(basketItemOrdered, productItem.Price, item.Quantity, productItem.Picture);
               
                if (productItem.DiscountedPrice != null)
                {
                    orderItem.Price = (decimal)productItem.DiscountedPrice;
                }                
                
                orderItems.Add(orderItem);

              //  productItem.StockQuantity = productItem.StockQuantity - item.Quantity;

                await _unitOfWork.OrderRepository.FillingItemWarehousesQuantity(productItem.Id, item.Quantity);

            }

            var shippingOption = await _unitOfWork.ShippingOptionRepository.GetShippingOptionById(shippingOptionId);

            var paymentOption = await _unitOfWork.ShippingOptionRepository.GetPaymentOptionById(paymentOptionId);
 
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            var existingOrder = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(basket.PaymentIntentId);

            if (existingOrder != null)
            {
                _unitOfWork.OrderRepository.DeleteCustomerOrder(existingOrder);

                await _paymentService.CreatingOrUpdatingPaymentIntent(basket.PaymentIntentId);
            }
            
            var customerOrder = new CustomerOrder(orderItems, buyerEmail, shippingAddress, shippingOption,
            paymentOption, subtotal, basket.PaymentIntentId);
            _unitOfWork.OrderRepository.CreateCustomerOrder(customerOrder);

            if (await _unitOfWork.SaveAsync()) return customerOrder;

            return null;        
        }

        public async Task<List<Country>> GetCountries()
        {
            return await _unitOfWork.OrderRepository.GetCountries();
        }

        public async Task<Country> GetCountryById(int id)
        {
            return await _unitOfWork.OrderRepository.GetCountryById(id);
        }

        public string GetCountryName(int id)
        {
            return _unitOfWork.OrderRepository.GetCountryName(id);
        }

        public async Task<CustomerOrder> GetOrderById(int id, string customerEmail)
        {
            return await _unitOfWork.OrderRepository.GetOrderById(id, customerEmail);
        }

        public async Task<List<CustomerOrder>> GetOrdersForCustomer(string customerEmail)
        {
            return await _unitOfWork.OrderRepository.GetOrdersForCustomer(customerEmail);
        }

        public async Task<List<PaymentOption>> GetPayingOptions()
        {
            return await _unitOfWork.OrderRepository.GetPayingOptions();
        }

        public string GetPaymentOptionName(int id)
        {
            return _unitOfWork.OrderRepository.GetPaymentOptionName(id);
        }

        public decimal GetShippingOptionPrice(int id)
        {
            return _unitOfWork.OrderRepository.GetShippingOptionPrice(id);
        }

        public async Task<List<ShippingOption>> GetShippingOptions()
        {
            return await _unitOfWork.OrderRepository.GetShippingOptions();
        }

        public async Task<PaymentOption> GetStripePaymentOption()
        {
            return await _unitOfWork.OrderRepository.GetStripePaymentOption();
        }

        public async Task FillingItemWarehousesQuantity(int id, int basketItemQuantity)
        {
            await _unitOfWork.OrderRepository.FillingItemWarehousesQuantity(id, basketItemQuantity);
        }

    }
}






