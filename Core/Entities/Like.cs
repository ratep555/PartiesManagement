namespace Core.Entities
{
    public class Like
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}