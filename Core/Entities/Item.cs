using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string Picture { get; set; }
        public int? StockQuantity { get; set; }

        // ovo ne mora biti nullable, ti si stavio jer ti zgodno sada
        public bool? NotReturnable { get; set; }
        public bool? HasDiscountsApplied { get; set; }


        public ICollection<ItemCategory> ItemCategories { get; set; }
        public ICollection<ItemManufacturer> ItemManufacturers { get; set; }
        public ICollection<ItemTag> ItemTags { get; set; }
        public ICollection<ItemDiscount> ItemDiscounts { get; set; }

    }
}







