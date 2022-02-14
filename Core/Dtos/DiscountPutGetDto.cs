using System.Collections.Generic;

namespace Core.Dtos
{
    public class DiscountPutGetDto
    {
        public DiscountDto Discount { get; set; }
        public IEnumerable<ItemDto> SelectedItems { get; set; }
        public IEnumerable<ItemDto> NonSelectedItems { get; set; }
        public IEnumerable<CategoryDto> SelectedCategories { get; set; }
        public IEnumerable<CategoryDto> NonSelectedCategories { get; set; }
    }
}