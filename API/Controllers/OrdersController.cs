using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.ErrorHandling;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Entities.Order;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Authorize(Policy = "RequireVisitorRole")]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IPdfService _pdfService;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork,
            IEmailService emailService, IConfiguration config, IPdfService pdfService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _config = config;
            _pdfService = pdfService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerOrder>> CreateOrder(OrderDto orderDto)
        {

           var email = User.RetrieveEmailFromPrincipal();

           var address = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

           var order = await _orderService.CreateOrder(email, orderDto.ShippingOptionId,
           orderDto.BasketId, address);

           if(order == null) 
           return BadRequest("Problem creating order");

           return Ok(order);
        }
        
        // ova šljaka
        [HttpPost("oki")]
        public async Task<ActionResult<CustomerOrder>> CreateOrder1(OrderDto1 orderDto)
        {
           var email = User.RetrieveEmailFromPrincipal();

           var country = await _orderService.GetCountryById(orderDto.ShippingAddress.CountryId);

           var address = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

           var order = await _orderService.CreateOrder(email, orderDto.ShippingOptionId,
           orderDto.BasketId, address);

           if(order == null) 
           return BadRequest("Problem creating order");

           return Ok(order);
        }

        // ovo je pokušaj sa mailom, obje dolje koristiš, funkcioniraju:)
        [HttpPost("okimail")]
        public async Task<ActionResult<CustomerOrder>> CreateOrder2(OrderDto2 orderDto)
        {
            var email = User.RetrieveEmailFromPrincipal();

            // var country = await _orderService.GetCountryById(orderDto.ShippingAddress.CountryId);

            var shippingOptionPrice = _orderService.GetShippingOptionPrice(orderDto.ShippingOptionId);

            var paymentOptionName = _orderService.GetPaymentOptionName(orderDto.PaymentOptionId);

            var address = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

            if (await _orderService.CheckIfBasketItemQuantityExceedsItemStackQuantity(orderDto.BasketId))
            {
                return BadRequest("Quantity in your basket excceds item's stack quantity!");
            }

            var order = await _orderService.CreateOrder2(email, orderDto.ShippingOptionId, orderDto.PaymentOptionId,
                orderDto.BasketId, address);
                        
            var total = order.Subtotal + shippingOptionPrice;

           if (paymentOptionName == "Cash on Delivery (COD)")
           {
                // order.PaymentIntentId = null;

                string url = $"{_config["AngularAppUrl"]}/orders/{order.Id}";

                await _emailService.SendEmailAsync(email, 
                "Order confirmation", $"<h1>Thank you for your order</h1>" +
                $"<p>Your order will be shipped in accordance with your selected shipping preferences." +
                $" You can view details of your order by <a href='{url}'>Clicking here</a></p>");
           }

           if (paymentOptionName == "General Card Slip")
           {
                // order.PaymentIntentId = null;

                _emailService.GeneratePdf(order.Id, total, orderDto.ShippingAddress.FirstName, 
                    orderDto.ShippingAddress.LastName);

                string url = $"{_config["AngularAppUrl"]}/orders/{order.Id}";

                await _emailService.SendEmailAsync1(email, 
                "Order confirmation", $"<h1>Thank you for your order</h1>" +
                $"<h3>Please pay {total}</h3>" +
                $"<p>Your order will be shipped in accordance with your selected shipping preferences" +
                $"once the payment is completed. You can view details of your order by <a href='{url}'>Clicking here</a></p>", order.Id);
           }

           await _unitOfWork.SaveAsync();

           if(order == null) return BadRequest("Problem creating order");

           return Ok(order);
        }
        // ovo je stripe
        [HttpPost("okimail1")]
        public async Task<ActionResult<CustomerOrder>> CreateOrder3(OrderDto2 orderDto)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var shippingOptionPrice = _orderService.GetShippingOptionPrice(orderDto.ShippingOptionId);

            var paymentOptionName = _orderService.GetPaymentOptionName(orderDto.PaymentOptionId);

            var address = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

            if (await _orderService.CheckIfBasketItemQuantityExceedsItemStackQuantity(orderDto.BasketId))
            {
                return BadRequest("Quantity in your basket excceds item's stack quantity!");
            }

            var order = await _orderService.CreateOrder3(email, orderDto.ShippingOptionId, orderDto.PaymentOptionId,
                orderDto.BasketId, address);

           await _unitOfWork.SaveAsync();

           if(order == null) return BadRequest("Problem creating order");

           return Ok(order);
        }

        [AllowAnonymous]
        [HttpGet("shippingoptions")]
        public async Task<ActionResult<IEnumerable<ShippingOptionDto>>> GetShippingOptions()
        {
            var list = await _orderService.GetShippingOptions();

            var shippingoptions = _mapper.Map<IEnumerable<ShippingOptionDto>>(list);

            return Ok(shippingoptions);        
        }

        [AllowAnonymous]
        [HttpGet("payingoptions")]
        public async Task<ActionResult<IEnumerable<PayingOptionDto>>> GetPayingOptions()
        {
            var list = await _orderService.GetPayingOptions();

            var payingoptions = _mapper.Map<IEnumerable<PayingOptionDto>>(list);

            return Ok(payingoptions);        
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            var list = await _orderService.GetCountries();

            var countries = _mapper.Map<IEnumerable<CountryDto>>(list);

            return Ok(countries);        
        }

        [AllowAnonymous]
        [HttpGet("stripepay")]
        public async Task<ActionResult<PayingOptionDto>> GetStripePayingOption()
        {
            var stripe = await _orderService.GetStripePaymentOption();

            return Ok(stripe);        
        }

        [AllowAnonymous]
        [HttpGet("pdf")]
        public async Task<ActionResult> GeneratePdf()
        {
            int orderNo = 20;

            _pdfService.GeneratePdf(orderNo);

            return Ok();        
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderForCustomerListDto>>> GetOrdersForCustomer()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForCustomer(email);

            var ordersDto = _mapper.Map<List<OrderForCustomerListDto>>(orders);

            foreach (var item in ordersDto)
            {
                item.ShippingAddress.Country = _orderService.GetCountryName(item.ShippingAddress.CountryId);
            }

            return ordersDto;;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderForCustomerListDto>> GetOrderByIdForCustomer(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var order = await _orderService.GetOrderById(id, email);

            if (order == null) return NotFound(new ServerResponse(404)); 

            var orderDto = _mapper.Map<OrderForCustomerListDto>(order);
            orderDto.ShippingAddress.Country = _orderService.GetCountryName(orderDto.ShippingAddress.CountryId);

            return orderDto;
        } 

       /*  [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderEditDto orderDto)
        {
            var order = await _unitOfWork.ItemRepository.GetOrderByIdForEditing(id);

            if (order == null) return BadRequest("Bad request!");

            order.OrderStatus = (OrderStatus) Enum.Parse(typeof(OrderStatus), orderDto.OrderStatus);
            
            await _unitOfWork.SaveAsync();

            return NoContent();
        } */

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderEditDto orderDto)
        {
             var order = await _unitOfWork.ItemRepository.GetOrderByIdForEditing(id);

            if (order == null) return BadRequest("Bad request!");  

            order.OrderStatus1Id = orderDto.OrderStatus1Id;

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("orderstatuses")]
        public async Task<ActionResult<List<OrderStatusDto>>> GetAllOrderStatuses()
        {
            var list = await _unitOfWork.ItemRepository.GetAllOrderStatuses();

            return _mapper.Map<List<OrderStatusDto>>(list);
        }


        // warehouse attempt
        [HttpPut("warehouse/{id}")]
        public async Task<ActionResult> FillingWarehousesQuantity(int id)
        {
            await _unitOfWork.OrderRepository.FillingItemWarehousesQuantity(id, 6);

            return NoContent();
        } 
     
    }
}








