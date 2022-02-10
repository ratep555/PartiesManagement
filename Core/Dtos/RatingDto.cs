using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class RatingDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        public int ApplicationUserId { get; set; }
        public int ItemId { get; set; }
    }
}