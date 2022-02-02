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
            
            CreateMap<ItemCreateEditDto, Item>()
                .ForMember(x => x.Picture, options => options.Ignore());

        }
    }
}










