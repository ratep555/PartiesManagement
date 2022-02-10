using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinimumOrderValue { get; set; }
        public ICollection<ItemDiscount> ItemDiscounts { get; set; }

    }
}