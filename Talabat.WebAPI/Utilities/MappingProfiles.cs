using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.WebAPI.DTOs;

namespace Talabat.WebAPI.Utilities
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(PDTO => PDTO.brand, O => O.MapFrom(P => P.brand.Name))
                .ForMember(PDTO => PDTO.category, O => O.MapFrom(P => P.category.Name))
                .ForMember(PDTO => PDTO.PictureURL, O => O.MapFrom<ImageURLResolver<Product, ProductDTO>>());

            CreateMap<Core.Domain_Models.Identity.Address, AddressDTO>().ReverseMap();

            CreateMap<BasketItemDTO, BasketItem>().ReverseMap()
                .ForMember(BIDTO => BIDTO.pictureUrl, BI => BI.MapFrom<ImageURLResolver<BasketItem, BasketItemDTO>>());
            CreateMap<BasketDTO, Basket>().ReverseMap();
            CreateMap<AddressDTO, Core.Domain_Models.Order_Module.Address>();
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(OTRDTO => OTRDTO.DeliveryMethod_name, O => O.MapFrom(O => O.DeliveryMethod.ShortName))
                .ForMember(OTRDTO => OTRDTO.DeliveryMethod_cost, O => O.MapFrom(O => O.DeliveryMethod.Cost))
                .ForMember(OTRDTO => OTRDTO .Status, O => O.MapFrom(O => O.Status));
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(OIDTO => OIDTO.ProductID, O => O.MapFrom(OI => OI.OrderedProduct.ID))
                .ForMember(OIDTO => OIDTO.Name, O => O.MapFrom(OI => OI.OrderedProduct.Name))
                .ForMember(OIDTO => OIDTO.ImageURL, O => O.MapFrom<ImageURLResolver<OrderItem, OrderItemDTO>>());

        }
    }
}
