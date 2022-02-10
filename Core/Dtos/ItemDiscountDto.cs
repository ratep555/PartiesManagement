namespace Core.Dtos
{
    public class ItemDiscountDto
    {
         public int ItemId { get; set; }
        public int DiscountId { get; set; }
        public bool IsAppliedOnItem { get; set; }
    }
}