using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Order;

namespace Core.Entities
{
    public class CustomerOrder : BaseEntity
    {
        public CustomerOrder()
        {
        }

        public CustomerOrder(
            ICollection<OrderItem> orderItems,
            string customerEmail,
            ShippingAddress shippingAddress,
            ShippingOption shippingOption,
            decimal subtotal,
            string paymentIntentId)
        {
            OrderItems = orderItems;
            CustomerEmail = customerEmail;
            ShippingAddress = shippingAddress;
            ShippingOption = shippingOption;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }
        public CustomerOrder(
            ICollection<OrderItem> orderItems,
            string customerEmail,
            ShippingAddress shippingAddress,
            ShippingOption shippingOption,
            PaymentOption paymentOption,
            decimal subtotal,
            string paymentIntentId)
        {
            OrderItems = orderItems;
            CustomerEmail = customerEmail;
            ShippingAddress = shippingAddress;
            ShippingOption = shippingOption;
            PaymentOption = paymentOption;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }
        
        public string CustomerEmail { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        public DateTimeOffset DateOfCreation { get; set; } = DateTimeOffset.Now;
        public decimal Subtotal { get; set; }
        public string PhoneNumber { get; set; }
        public bool Shipped { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.PendingPayment;
        
        public int ShippingOptionId { get; set; }
        public ShippingOption ShippingOption { get; set; }

        public int? PaymentOptionId { get; set; }
        public PaymentOption PaymentOption { get; set; }

        public string PaymentIntentId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        
        public int? PaymentStatus1Id { get; set; }
        public PaymentStatus1 PaymentStatus1 { get; set; }

        public int? OrderStatus1Id { get; set; }
        public OrderStatus1 OrderStatus1 { get; set; }

        public string PaymentReport { get; set; }

        // razmisli treba li ti gettotal, radio si dva dto-a, možda da samo to
        // na klijentu zbrojiš
        public decimal GetTotal()
        {
            return Subtotal + ShippingOption.Price;
        }
    }
}











