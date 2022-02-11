using System;
using System.Collections.Generic;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Core.Entities
{
    public class DiscountCreateEditDto
    {
        public string Name { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal?>))]
        public decimal? MinimumOrderValue { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ItemsIds { get; set; }    }
}








