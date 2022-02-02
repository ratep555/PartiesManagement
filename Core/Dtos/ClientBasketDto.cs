using System.Collections.Generic;
using Core.Dtos;

namespace Core.Entities
{
    public class ClientBasketDto
    {
        public string Id { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public int? ShippingOptionId { get; set; }
        public int? PaymentOptionId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}