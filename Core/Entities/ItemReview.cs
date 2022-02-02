using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ItemReview : BaseEntity
    {
        [Range(1, 5)]
        public int Rate { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

    }
}