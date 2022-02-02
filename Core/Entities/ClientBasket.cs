using System.Collections.Generic;

namespace Core.Entities
{
    public class ClientBasket
    {
        public ClientBasket()
        {
        }
        public ClientBasket(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public int? ShippingOptionId { get; set; }
        public int? PaymentOptionId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
        
    }
}