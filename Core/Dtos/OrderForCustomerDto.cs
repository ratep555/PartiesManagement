using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Order;

namespace Core.Dtos
{
    public class OrderForCustomerDto
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public DateTimeOffset DateOfCreation { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public string ShippingOption { get; set; }
        public decimal ShippingPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GetTotal { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
    }
}