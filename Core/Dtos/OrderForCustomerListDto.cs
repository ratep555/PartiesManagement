using System;
using System.Collections.Generic;
using Core.Entities.Order;

namespace Core.Dtos
{
    public class OrderForCustomerListDto
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public DateTimeOffset DateOfCreation { get; set; }
        public ShippingAddressToReturnDto1 ShippingAddress { get; set; }
        public string ShippingOption { get; set; }
        public string PaymentOption { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GetTotal { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }

    }
}







