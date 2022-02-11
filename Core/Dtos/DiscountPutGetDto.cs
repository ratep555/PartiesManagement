using System.Collections.Generic;

namespace Core.Dtos
{
    public class DiscountPutGetDto
    {
        public DiscountDto Discount { get; set; }
        public IEnumerable<ItemDto> SelectedItems { get; set; }
        public IEnumerable<ItemDto> NonSelectedItems { get; set; }
    }
}