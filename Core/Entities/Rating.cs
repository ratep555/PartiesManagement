using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Rating : BaseEntity
    {
        [Range(1, 5)]
        public int Rate { get; set; }

        public int ApplicationUserId { get; set; }
        public ApplicationUser Customer { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}