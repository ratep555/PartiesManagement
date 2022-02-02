using System.IO;
using System.Threading.Tasks;
using API.ErrorHandling;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IConfiguration _config;
        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger,
            IConfiguration config)
        {
            _logger = logger;
            _paymentService = paymentService;
            _config = config;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<ClientBasket>> CreatingOrUpdatingPaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreatingOrUpdatingPaymentIntent(basketId);

            if (basket == null) return BadRequest(new ServerResponse(400, "Problem creating or updating basket"));

            return basket;
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            //ovo je stripe, hover over constructevent
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], 
                _config["StripeSettings:WhSecret"]);

            PaymentIntent intent;
            CustomerOrder order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", intent.Id);
                    order = await _paymentService.UpdatingOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", intent.Id);
                    order = await _paymentService.UpdatingOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Payment Failed: ", order.Id);
                    break;
            }

            return new EmptyResult();
        }

    
    }
}












