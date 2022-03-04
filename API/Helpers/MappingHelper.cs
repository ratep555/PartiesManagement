using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.Birthday;
using Core.Entities;
using Core.Entities.Birthday;
using Core.Entities.Blogs;
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
                .ForMember(d => d.Categories, o => o.MapFrom(MapForCategories1))
                .ForMember(d => d.Manufacturers, o => o.MapFrom(MapForManufacturers));
            
            CreateMap<OrderEditDto, CustomerOrder>();
 
            CreateMap<DiscountCreateEditDto, Discount>()
                .ForMember(x => x.ItemDiscounts, options => options.MapFrom(MapDiscountItems))
                .ForMember(x => x.CategoryDiscounts, options => options.MapFrom(MapDiscountCategories))
                .ForMember(x => x.ManufacturerDiscounts, options => options.MapFrom(MapDiscountManufacturers));

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
            CreateMap<Manufacturer1, Manufacturer1Dto>().ReverseMap();
            
            // pazi na redoslijed kojim to navodiš, ovo te zezalo, nemoj duplati!
           // CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
            CreateMap<OrderStatus1, OrderStatusDto>().ReverseMap();
            CreateMap<Tag, TagDto>().ReverseMap();

            CreateMap<ItemWarehouse, ItemWarehouseDto>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s.Item.Name))
                .ForMember(d => d.Warehouse, o => o.MapFrom(s => s.Warehouse.WarehouseName));
            
            CreateMap<ItemWarehouseDto, ItemWarehouse>();


            CreateMap<ItemWarehouseCreateEditDto, ItemWarehouse>();

            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Country.Name));
            
            //birthdays
            CreateMap<BirthdayCreateDto, Birthday1>();
            CreateMap<BirthdayEditDto, Birthday1>();
            CreateMap<ServiceIncluded, ServiceIncludedDto>().ReverseMap();

            CreateMap<Birthday1, BirthdayDto>()
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location.City))
                .ForMember(d => d.BirthdayPackage, o => o.MapFrom(s => s.BirthdayPackage.PackageName))
                .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.OrderStatus1.Name));
            
            // birthdaypackages
            CreateMap<BirthdayPackage, BirthdayPackageDto>()
                .ForMember(d => d.ServicesIncluded, o => o.MapFrom(MapForServicesIncluded))
                .ForMember(d => d.Discounts, o => o.MapFrom(MapForDiscounts));
            
            CreateMap<BirthdayPackageCreateEditDto, BirthdayPackage>()
                .ForMember(x => x.Picture, options => options.Ignore())
                .ForMember(x => x.BirthdayPackageDiscounts, options => options.MapFrom(MapBirthdayPackageDisounts))
                .ForMember(x => x.BirthdayPackageServices, options => options.MapFrom(MapBirthdayPackageServices));
            
             
            // location
            CreateMap<Location1, LocationDto>()
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Country.Name))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Location.Y))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Location.X)); 

            CreateMap<LocationCreateEditDto, Location1>()
               .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            // messages
            CreateMap<MessageCreateDto, Message>().ReverseMap();

            // servicesincluded
            CreateMap<ServiceIncluded, ServiceIncludedDto>();

            CreateMap<ServiceIncludedCreateEditDto, ServiceIncluded>()
                .ForMember(x => x.Picture, options => options.Ignore());

            // blogs
            CreateMap<Blog, BlogDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.ApplicationUser.UserName));


            CreateMap<BlogCreateEditDto, Blog>()
                .ForMember(x => x.Picture, options => options.Ignore());
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

        private List<Manufacturer1Discount> MapDiscountManufacturers(DiscountCreateEditDto discountDto, Discount discount)
        {
            var result = new List<Manufacturer1Discount>();

            if (discountDto.ManufacturersIds == null) { return result; }

            foreach (var id in discountDto.ManufacturersIds)
            {
                result.Add(new Manufacturer1Discount() { Manufacturer1Id = id });
            }
            return result;
        }

        private List<BirthdayPackageDiscount> MapBirthdayPackageDisounts(
                BirthdayPackageCreateEditDto birthdayDto, BirthdayPackage birthdayPackage)
        {
            var result = new List<BirthdayPackageDiscount>();

            if (birthdayDto.DiscountsIds == null) { return result; }

            foreach (var id in birthdayDto.DiscountsIds)
            {
                result.Add(new BirthdayPackageDiscount() { DiscountId = id });
            }
            return result;
        }

        private List<BirthdayPackageService> MapBirthdayPackageServices(
                BirthdayPackageCreateEditDto birthdayDto, BirthdayPackage birthdayPackage)
        {
            var result = new List<BirthdayPackageService>();

            if (birthdayDto.ServicesIds == null) { return result; }

            foreach (var id in birthdayDto.ServicesIds)
            {
                result.Add(new BirthdayPackageService() { ServiceIncludedId = id });
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

        private List<Manufacturer1Dto> MapForManufacturers(Discount discount, DiscountDto discountDto)
        {
            var result = new List<Manufacturer1Dto>();

            if (discount.ManufacturerDiscounts != null)
            {
                foreach (var manufacturer in discount.ManufacturerDiscounts)
                {
                    result.Add(new Manufacturer1Dto() { Id = manufacturer.Manufacturer1Id, 
                    Name = manufacturer.Manufacturer1.Name });
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

        // birthdays
        private List<ServiceIncludedDto> MapForServicesIncluded(
            BirthdayPackage birthdayPackage, BirthdayPackageDto birthdayPackageDto)
        {
            var result = new List<ServiceIncludedDto>();

            if (birthdayPackage.BirthdayPackageServices != null)
            {
                foreach (var service in birthdayPackage.BirthdayPackageServices)
                {
                    result.Add(new ServiceIncludedDto() { Id = service.ServiceIncludedId, 
                    Name = service.ServiceIncluded.Name });
                }
            }
            return result;
        }
        private List<DiscountDto> MapForDiscounts(
            BirthdayPackage birthdayPackage, BirthdayPackageDto birthdayPackageDto)
        {
            var result = new List<DiscountDto>();

            if (birthdayPackage.BirthdayPackageDiscounts != null)
            {
                foreach (var discount in birthdayPackage.BirthdayPackageDiscounts)
                {
                    result.Add(new DiscountDto() { Id = discount.DiscountId, 
                    Name = discount.Discount.Name });
                }
            }
            return result;
        }

        private List<CategoryDto> MapForCategories7(Item item, ItemDto itemDto)
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

    }
}










