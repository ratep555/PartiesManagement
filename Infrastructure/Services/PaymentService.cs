using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _config = config;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientBasket> CreatingOrUpdatingPaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasket(basketId);

            if (basket == null) return null;
            
            var shippingPrice = 0m;

            if (basket.ShippingOptionId.HasValue)
            {
                var shippingOption = await _unitOfWork.OrderRepository
                //(int) kako bi mogao convert int? to int
                .FindShippingOptionById((int)basket.ShippingOptionId);

                shippingPrice = shippingOption.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ItemRepository.GetItemById(item.Id);

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    //stripe does not take decimals, it takes numbers in long format so
                    //it has to be like this, we have to cast everything to long (multiplying by 100)
                    Amount = (long) basket.BasketItems.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"}
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long) basket.BasketItems.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.EditBasket(basket);

            return basket;
        }

        public async Task<CustomerOrder> UpdatingOrderPaymentFailed(string paymentIntentId)
        {
            var order = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(paymentIntentId);;

            if (order == null) return null;

            order.PaymentStatus = PaymentStatus.PaymentFailed;

            await _unitOfWork.SaveAsync();

            return order;        
        }

        public async Task<CustomerOrder> UpdatingOrderPaymentSucceeded(string paymentIntentId)
        {
            var order = await _unitOfWork.OrderRepository.FindOrderByPaymentIntentId(paymentIntentId);;

            if (order == null) return null;

            order.PaymentStatus = PaymentStatus.PaymentReceived;
            _unitOfWork.OrderRepository.UpdateCustomerOrder(order);

            await _unitOfWork.SaveAsync();

            return order;           }
    }
}