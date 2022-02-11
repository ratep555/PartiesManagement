
using System;
using System.Collections.Generic;

namespace Core.Dtos
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinimumOrderValue { get; set; }
        public List<ItemDto> Items { get; set; }

    }
}