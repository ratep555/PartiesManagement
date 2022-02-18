using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public decimal? MinimumOrderValue { get; set; }
        public ICollection<ItemDiscount> ItemDiscounts { get; set; }
        public ICollection<CategoryDiscount> CategoryDiscounts { get; set; }
        public ICollection<Manufacturer1Discount> ManufacturerDiscounts { get; set; }

    }
}