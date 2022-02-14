using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Core.Entities
{
    public class DiscountCreateEditDto
    {
        public int iD { get; set; }
        public string Name { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal DiscountPercentage { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal?>))]
        public decimal? MinimumOrderValue { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ItemsIds { get; set; } 
        

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CategoriesIds { get; set; }    
    }
}








