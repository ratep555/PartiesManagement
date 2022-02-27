using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ItemDiscount
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }

    }
}