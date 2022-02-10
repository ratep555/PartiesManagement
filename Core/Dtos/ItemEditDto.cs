using System.Collections.Generic;
using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Dtos
{
    public class ItemEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal Price { get; set; }

        public IFormFile Picture { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CategoriesIds { get; set; }
        

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> DiscountsIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ManufacturersIds { get; set; }
        

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> TagsIds { get; set; }

    }
}