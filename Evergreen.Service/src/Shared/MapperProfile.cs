using AutoMapper;
using Evergreen.Core.src.Entity;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Shared;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserReadDTO>();
        CreateMap<User, UserSimpleReadDTO>();
        CreateMap<User, ProfileReadDTO>()
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(s => s.Orders));
        CreateMap<UserCreateDTO, User>();
        CreateMap<UserUpdateDTO, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
        CreateMap<UserWithRoleCreateDTO, User>();

        CreateMap<Category, CategoryReadDTO>();
        CreateMap<CategoryCreateDTO, Category>();
        CreateMap<CategoryUpdateDTO, Category>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<ProductDetailsCreateDTO, ProductDetails>();
        CreateMap<ProductDetails, ProductDetailsReadDTO>();
        CreateMap<ProductDetailsUpdateDTO, ProductDetails>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<Image, ImageReadDTO>();
        CreateMap<Image, ProductImageReadDTO>();
        CreateMap<ImageCreateDTO, Image>();
        CreateMap<ImageUpdateDTO, Image>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
        
        CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages))
            .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(s => s.ProductDetails));
        CreateMap<Product, ProductSimpleReadDTO>();
        CreateMap<ProductCreateDTO, Product>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(s => s.ProductDetails))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages));
        CreateMap<ProductUpdateDTO, Product>()
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
            .ForMember(dest => dest.LatinName, opt => opt.Condition(src => src.LatinName != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price > 0));
        
        CreateMap<OrderProduct, OrderProductReadDTO>();
        CreateMap<OrderProductCreateDTO, OrderProduct>();

        CreateMap<Order, OrderReadDTO>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(s => s.OrderDetails))
            .ForMember(dest => dest.User, opt => opt.MapFrom(s => s.User));
        CreateMap<OrderCreateDTO, Order>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(s => s.OrderDetails));
        CreateMap<OrderUpdateDTO, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
    }
}