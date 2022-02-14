using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Entities.Order;
using NetTopologySuite.Geometries;

namespace API.Helpers
{
    public class MappingHelper : Profile
    {
        public MappingHelper(GeometryFactory geometryFactory)
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.DisplayName));
            
            CreateMap<ShippingAddressDto, ShippingAddress>();

            CreateMap<ShippingOption, ShippingOptionDto>().ReverseMap();
            CreateMap<PaymentOption, PayingOptionDto>().ReverseMap();


            CreateMap<Country, CountryDto>().ReverseMap();

            CreateMap<ShippingAddressDto1, ShippingAddress>();

            CreateMap<ShippingAddress, ShippingAddressToReturnDto1>();

            CreateMap<Address, ShippingAddressDto1>().ReverseMap();
            CreateMap<LoginDto, ApplicationUser>().ReverseMap();
            CreateMap<ClientBasketDto, ClientBasket>();
            
            CreateMap<Item, ItemDto>().ReverseMap();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<CustomerOrder, OrderForCustomerDto>()
                .ForMember(d => d.ShippingOption, o => o.MapFrom(s => s.ShippingOption.Name))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.ShippingOption.Price));

            CreateMap<CustomerOrder, OrderForCustomerListDto>()
                .ForMember(d => d.ShippingOption, o => o.MapFrom(s => s.ShippingOption.Name))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.ShippingOption.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ItemId, o => o.MapFrom(s =>  s.BasketItemOrdered.BasketItemOrderedId))
                .ForMember(d => d.ItemName, o => o.MapFrom(s =>  s.BasketItemOrdered.BasketItemOrderedName));
            
            CreateMap<Discount, DiscountDto>()
                .ForMember(d => d.Items, o => o.MapFrom(MapForItems))
                .ForMember(d => d.Categories, o => o.MapFrom(MapForCategories1));
            
            CreateMap<DiscountCreateEditDto, Discount>()
                .ForMember(x => x.ItemDiscounts, options => options.MapFrom(MapDiscountItems))
                .ForMember(x => x.CategoryDiscounts, options => options.MapFrom(MapDiscountCategories));

            CreateMap<Item, ItemDto>()
                .ForMember(d => d.Categories, o => o.MapFrom(MapForCategories))
                .ForMember(d => d.Discounts, o => o.MapFrom(MapForDiscounts))
                .ForMember(d => d.Manufacturers, o => o.MapFrom(MapForManufacturers))
                .ForMember(d => d.Tags, o => o.MapFrom(MapForTags));
            
            CreateMap<ItemCreateEditDto, Item>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.ItemCategories, options => options.MapFrom(MapItemCategories))
                .ForMember(x => x.ItemDiscounts, options => options.MapFrom(MapItemDiscounts))
                .ForMember(x => x.ItemManufacturers, options => options.MapFrom(MapItemManufacturers))
                .ForMember(x => x.ItemTags, options => options.MapFrom(MapItemTags));

            CreateMap<ItemEditDto, Item>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.ItemCategories, options => options.MapFrom(MapItemCategories1))
                .ForMember(x => x.ItemDiscounts, options => options.MapFrom(MapItemDiscounts1))
                .ForMember(x => x.ItemManufacturers, options => options.MapFrom(MapItemManufacturers1))
                .ForMember(x => x.ItemTags, options => options.MapFrom(MapItemTags1));

            CreateMap<Category, CategoryDto>().ReverseMap();
            // pazi na redoslijed kojim to navodiš, ovo te zezalo, nemoj duplati!
           // CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
            CreateMap<Tag, TagDto>().ReverseMap();
        }

        private List<ItemDiscount> MapDiscountItems(DiscountCreateEditDto discountDto, Discount discount)
        {
            var result = new List<ItemDiscount>();

            if (discountDto.ItemsIds == null) { return result; }

            foreach (var id in discountDto.ItemsIds)
            {
                result.Add(new ItemDiscount() { ItemId = id });
            }
            return result;
        }

        private List<CategoryDiscount> MapDiscountCategories(DiscountCreateEditDto discountDto, Discount discount)
        {
            var result = new List<CategoryDiscount>();

            if (discountDto.CategoriesIds == null) { return result; }

            foreach (var id in discountDto.CategoriesIds)
            {
                result.Add(new CategoryDiscount() { CategoryId = id });
            }
            return result;
        }

        private List<ItemCategory> MapItemCategories(ItemCreateEditDto itemDto, Item item)
        {
            var result = new List<ItemCategory>();

            if (itemDto.CategoriesIds == null) { return result; }

            foreach (var id in itemDto.CategoriesIds)
            {
                result.Add(new ItemCategory() { CategoryId = id });
            }
            return result;
        }

        private List<ItemDiscount> MapItemDiscounts(ItemCreateEditDto itemDto, Item item)
        {
            var result = new List<ItemDiscount>();

            if (itemDto.DiscountsIds == null) { return result; }

            foreach (var id in itemDto.DiscountsIds)
            {
                result.Add(new ItemDiscount() { DiscountId = id });
            }
            return result;
        }
        private List<ItemCategory> MapItemCategories1(ItemEditDto itemDto, Item item)
        {
            var result = new List<ItemCategory>();

            if (itemDto.CategoriesIds == null) { return result; }

            foreach (var id in itemDto.CategoriesIds)
            {
                result.Add(new ItemCategory() { CategoryId = id });
            }
            return result;
        }

        private List<ItemDiscount> MapItemDiscounts1(ItemEditDto itemDto, Item item)
        {
            var result = new List<ItemDiscount>();

            if (itemDto.DiscountsIds == null) { return result; }

            foreach (var id in itemDto.DiscountsIds)
            {
                result.Add(new ItemDiscount() { DiscountId = id });
            }
            return result;
        }

        private List<ItemManufacturer> MapItemManufacturers(ItemCreateEditDto itemDto, Item item)
        {
            var result = new List<ItemManufacturer>();

            if (itemDto.ManufacturersIds == null) { return result; }

            foreach (var id in itemDto.ManufacturersIds)
            {
                result.Add(new ItemManufacturer() { ManufacturerId = id });
            }
            return result;
        }

        private List<ItemManufacturer> MapItemManufacturers1(ItemEditDto itemDto, Item item)
        {
            var result = new List<ItemManufacturer>();

            if (itemDto.ManufacturersIds == null) { return result; }

            foreach (var id in itemDto.ManufacturersIds)
            {
                result.Add(new ItemManufacturer() { ManufacturerId = id });
            }
            return result;
        }

        private List<ItemTag> MapItemTags(ItemCreateEditDto itemDto, Item item)
        {
            var result = new List<ItemTag>();

            if (itemDto.TagsIds == null) { return result; }

            foreach (var id in itemDto.TagsIds)
            {
                result.Add(new ItemTag() { TagId = id });
            }
            return result;
        }
        private List<ItemTag> MapItemTags1(ItemEditDto itemDto, Item item)
        {
            var result = new List<ItemTag>();

            if (itemDto.TagsIds == null) { return result; }

            foreach (var id in itemDto.TagsIds)
            {
                result.Add(new ItemTag() { TagId = id });
            }
            return result;
        }

        private List<ItemDto> MapForItems(Discount discount, DiscountDto discountDto)
        {
            var result = new List<ItemDto>();

            if (discount.ItemDiscounts != null)
            {
                foreach (var item in discount.ItemDiscounts)
                {
                    result.Add(new ItemDto() { Id = item.ItemId, 
                    Name = item.Item.Name, Price = item.Item.Price });
                }
            }
            return result;
        }
        private List<CategoryDto> MapForCategories1(Discount discount, DiscountDto discountDto)
        {
            var result = new List<CategoryDto>();

            if (discount.CategoryDiscounts != null)
            {
                foreach (var category in discount.CategoryDiscounts)
                {
                    result.Add(new CategoryDto() { Id = category.CategoryId, 
                    Name = category.Category.Name });
                }
            }
            return result;
        }
        private List<CategoryDto> MapForCategories(Item item, ItemDto itemDto)
        {
            var result = new List<CategoryDto>();

            if (item.ItemCategories != null)
            {
                foreach (var category in item.ItemCategories)
                {
                    result.Add(new CategoryDto() { Id = category.CategoryId, 
                    Name = category.Category.Name });
                }
            }
            return result;
        }

        private List<DiscountDto> MapForDiscounts(Item item, ItemDto itemDto)
        {
            var result = new List<DiscountDto>();

            if (item.ItemDiscounts != null)
            {
                foreach (var discount in item.ItemDiscounts)
                {
                    result.Add(new DiscountDto() { Id = discount.DiscountId, 
                    Name = discount.Discount.Name, 
                    DiscountPercentage = discount.Discount.DiscountPercentage,
                    EndDate = discount.Discount.EndDate });
                }
            }
            return result;
        }

        private List<ManufacturerDto> MapForManufacturers(Item item, ItemDto itemDto)
        {
            var result = new List<ManufacturerDto>();

            if (item.ItemManufacturers != null)
            {
                foreach (var manufacturer in item.ItemManufacturers)
                {
                    result.Add(new ManufacturerDto() { Id = manufacturer.ManufacturerId, 
                    Name = manufacturer.Manufacturer.Name });
                }
            }
            return result;
        }

        private List<TagDto> MapForTags(Item item, ItemDto itemDto)
        {
            var result = new List<TagDto>();

            if (item.ItemTags != null)
            {
                foreach (var tag in item.ItemTags)
                {
                    result.Add(new TagDto() { Id = tag.TagId, Name = tag.Tag.Name });
                }
            }
            return result;
        }
    }
}










