using AutoMapper;
using Evergreen.Core.src.Entity;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Shared;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserReadDTO>();
        CreateMap<UserCreateDTO, User>();
        CreateMap<UserUpdateDTO, User>();
        CreateMap<UserWithRoleCreateDTO, User>();

        CreateMap<Category, CategoryReadDTO>();
        CreateMap<CategoryCreateDTO, Category>();

        CreateMap<ProductDetailsCreateDTO, ProductDetails>();
        CreateMap<ProductDetails, ProductDetailsReadDTO>();

        CreateMap<Image, ImageReadDTO>();
        CreateMap<ImageCreateDTO, Image>();
        
        CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages))
            .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(s => s.ProductDetails));
        CreateMap<ProductCreateDTO, Product>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(s => s.ProductDetails))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages));

        CreateMap<OrderProduct, OrderProductReadDTO>();
        CreateMap<OrderProductCreateDTO, OrderProduct>();

        CreateMap<Order, OrderReadDTO>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(s => s.OrderDetails));
        CreateMap<OrderCreateDTO, Order>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(s => s.OrderDetails));
        CreateMap<OrderUpdateDTO, Order>();
    }
}