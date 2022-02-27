using System.Collections.Generic;
using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Dtos.Birthday
{
    public class BirthdayPackageCreateEditDto
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<int>))]
        public int NumberOfParticipants { get; set; }
        

        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal Price { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal AdditionalBillingPerParticipant { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<int>))]
        public int Duration { get; set; }

        
        [ModelBinder(BinderType = typeof(TypeBinder<bool?>))]
        public decimal? DiscountedPrice { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<bool?>))]
        public bool? HasDiscountsApplied { get; set; }

        
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ServicesIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> DiscountsIds { get; set; }
    }
}


