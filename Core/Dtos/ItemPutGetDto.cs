using System.Collections.Generic;

namespace Core.Dtos
{
    public class ItemPutGetDto
    {
        public ItemDto Item { get; set; }
        public IEnumerable<CategoryDto> SelectedCategories { get; set; }
        public IEnumerable<CategoryDto> NonSelectedCategories { get; set; }
        public IEnumerable<DiscountDto> SelectedDiscounts { get; set; }
        public IEnumerable<DiscountDto> NonSelectedDiscounts { get; set; }
        public IEnumerable<ManufacturerDto> SelectedManufacturers { get; set; }
        public IEnumerable<ManufacturerDto> NonSelectedManufacturers { get; set; }
        public IEnumerable<TagDto> SelectedTags { get; set; }
        public IEnumerable<TagDto> NonSelectedTags { get; set; }
    }
}








